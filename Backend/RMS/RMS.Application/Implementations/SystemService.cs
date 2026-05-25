using RMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using RMS.Application.DTOs;
using RMS.Application.Helpers;
using RMS.Domain.Interfaces;
using RMS.Domain.Entities;
using RMS.Core.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RMS.Application.Interfaces;

namespace RMS.Application.Implementations
{
    public class SystemService : ISystemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchRepository _branchRepository;
        private readonly IUserRepository _userRepository;

        public SystemService(IUnitOfWork unitOfWork, IBranchRepository branchRepository, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _branchRepository = branchRepository;
            _userRepository = userRepository;
        }

        public async Task<ResponseDto<SystemStatusDto>> GetSystemStatusAsync()
        {
            var branchCount = await _branchRepository.GetQueryable().CountAsync();
            var mainBranch = await _branchRepository.GetQueryable().FirstOrDefaultAsync(b => b.IsMainBranch);

            return ResponseDto<SystemStatusDto>.CreateSuccessResponse(new SystemStatusDto
            {
                IsInitialized = branchCount > 0,
                OrganizationName = mainBranch?.BranchName
            });
        }

        public async Task<ResponseDto<bool>> InitializeSystemAsync(SystemInitializationDto initializationDto)
        {
            // Security check: Only initialize if the system is empty
            var status = await GetSystemStatusAsync();
            if (status.Data.IsInitialized)
            {
                return ResponseDto<bool>.CreateErrorResponse("System is already initialized.");
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 1. Create Main Branch (Headquarters)
                var mainBranch = new Branch
                {
                    BranchName = initializationDto.OrganizationName,
                    BranchCode = initializationDto.OrganizationCode ?? "HQ-01",
                    Address = initializationDto.Address,
                    CurrencyCode = initializationDto.CurrencyCode,
                    CurrencySymbol = initializationDto.CurrencySymbol,
                    IsMainBranch = true,
                    CreatedBy = "setup_wizard",
                    CreatedDate = DateTime.UtcNow
                };
                await _branchRepository.AddAsync(mainBranch);
                await _unitOfWork.SaveChangesAsync(); // Get the BranchID

                // 2. Create Master Admin User
                var (hash, salt) = PasswordHelper.CreatePasswordHash(initializationDto.AdminPassword);
                var adminUser = new User
                {
                    UserName = initializationDto.AdminUserName,
                    FullName = initializationDto.AdminFullName,
                    Email = initializationDto.AdminEmail,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    BranchID = mainBranch.BranchID,
                    Status = true,
                    CreatedBy = "setup_wizard",
                    CreatedDate = DateTime.UtcNow
                };
                await _userRepository.AddAsync(adminUser);

                // 3. Seed Basic Roles & Permissions
                // In a production app, we would seed the FULL permission set here.
                // For this implementation, we assume the base seeding (via EF Migrations) handles definitions, 
                // and we link the user to the existing Admin role (ID: 1).
                var userRole = new UserRole
                {
                    UserID = adminUser.Id,
                    RoleID = 1, // System Administrator
                    AssignedBy = "setup_wizard",
                    AssignedAt = DateTime.UtcNow
                };
                await _unitOfWork.GetRepository<UserRole>().AddAsync(userRole);

                // 4. Seed Global Settings
                var settings = new List<SystemSetting>
                {
                    new SystemSetting { SettingKey = "DefaultTaxRate", SettingValue = initializationDto.DefaultTaxRate.ToString(), SettingGroup = "Finance", SettingType = "Decimal", CreatedBy = "setup_wizard" },
                    new SystemSetting { SettingKey = "ServiceChargeRate", SettingValue = initializationDto.ServiceChargeRate.ToString(), SettingGroup = "Finance", SettingType = "Decimal", CreatedBy = "setup_wizard" },
                    new SystemSetting { SettingKey = "RestaurantLogo", SettingValue = initializationDto.LogoBase64 ?? "", SettingGroup = "Branding", SettingType = "Image", CreatedBy = "setup_wizard" },
                    new SystemSetting { SettingKey = "PrimaryColor", SettingValue = initializationDto.PrimaryColor, SettingGroup = "Branding", SettingType = "Color", CreatedBy = "setup_wizard" },
                    new SystemSetting { SettingKey = "SecondaryColor", SettingValue = initializationDto.SecondaryColor, SettingGroup = "Branding", SettingType = "Color", CreatedBy = "setup_wizard" },
                    new SystemSetting { SettingKey = "PageTitle", SettingValue = initializationDto.OrganizationName, SettingGroup = "Global", SettingType = "String", CreatedBy = "setup_wizard" }
                };
                foreach (var s in settings) await _unitOfWork.GetRepository<SystemSetting>().AddAsync(s);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ResponseDto<bool>.CreateSuccessResponse(true, "System initialized successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ResponseDto<bool>.CreateErrorResponse($"Setup Failed: {ex.Message}");
            }
        }
        public async Task<ResponseDto<bool>> SeedDemoDataAsync()
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 1. Create Extra Branches
                var londonBranch = new Branch { BranchName = "London Soho", BranchCode = "UK-01", Address = "45 Soho Square, London", CurrencyCode = "GBP", CurrencySymbol = "£", CreatedBy = "demo_seeder" };
                var dubaiBranch = new Branch { BranchName = "Dubai Marina", BranchCode = "UAE-01", Address = "Marina Walk, Dubai", CurrencyCode = "AED", CurrencySymbol = "د.إ", CreatedBy = "demo_seeder" };
                
                await _branchRepository.AddAsync(londonBranch);
                await _branchRepository.AddAsync(dubaiBranch);
                await _unitOfWork.SaveChangesAsync();

                // 2. Create Staff for all branches
                var hqStaff = new List<Staff> {
                    new Staff { StaffName = "Marco Polo", StaffRole = "Head Chef", HourlyRate = 35, CommissionPercentage = 2, BranchID = 1 },
                    new Staff { StaffName = "Elena Fisher", StaffRole = "Senior Waiter", HourlyRate = 18, CommissionPercentage = 5, BranchID = 1 },
                    new Staff { StaffName = "Sam Drake", StaffRole = "Waiter", HourlyRate = 15, CommissionPercentage = 3, BranchID = 1 }
                };
                var londonStaff = new List<Staff> {
                    new Staff { StaffName = "Gordon R.", StaffRole = "Executive Chef", HourlyRate = 50, CommissionPercentage = 5, BranchID = londonBranch.BranchID },
                    new Staff { StaffName = "Jane Watson", StaffRole = "Waiter", HourlyRate = 20, CommissionPercentage = 4, BranchID = londonBranch.BranchID }
                };
                foreach (var s in hqStaff.Concat(londonStaff)) await _unitOfWork.GetRepository<Staff>().AddAsync(s);
                await _unitOfWork.SaveChangesAsync();

                // 3. Create VIP Customers
                var vips = new List<Customer> {
                    new Customer { CustomerName = "Tony Stark", CustomerEmail = "tony@stark.com", TotalSpent = 4500, LoyaltyTier = "Gold", LoyaltyPoints = 4500, Status = true },
                    new Customer { CustomerName = "Bruce Wayne", CustomerEmail = "bruce@wayne.com", TotalSpent = 8200, LoyaltyTier = "Gold", LoyaltyPoints = 8200, Status = true },
                    new Customer { CustomerName = "Peter Parker", CustomerEmail = "peter@dailybugle.com", TotalSpent = 120, LoyaltyTier = "Bronze", LoyaltyPoints = 120, Status = true }
                };
                foreach (var c in vips) await _unitOfWork.GetRepository<Customer>().AddAsync(c);
                await _unitOfWork.SaveChangesAsync();

                // 4. Create Historical Sales (Last 30 Days)
                var random = new Random();
                var products = await _unitOfWork.GetRepository<Product>().GetAllAsync();
                if (products.Any())
                {
                    for (int i = 0; i < 50; i++)
                    {
                        var saleDate = DateTime.UtcNow.AddDays(-random.Next(0, 30));
                        var waiter = hqStaff[random.Next(0, hqStaff.Count)];
                        var branchId = random.Next(0, 2) == 0 ? 1 : londonBranch.BranchID;
                        
                        var sale = new Sale
                        {
                            SaleDate = saleDate,
                            TotalAmount = random.Next(50, 300),
                            PaymentMethod = random.Next(0, 2) == 0 ? "Cash" : "Card",
                            BranchID = branchId,
                            FinalAmount = 0, // Calculated below
                            CreatedBy = "demo_seeder",
                            CreatedDate = saleDate,
                            SaleDetails = new List<SaleDetail>()
                        };

                        var product = products[random.Next(0, products.Count)];
                        sale.SaleDetails.Add(new SaleDetail {
                            ProductID = product.Id,
                            Quantity = random.Next(1, 5),
                            UnitPrice = product.ProductPrice,
                            TotalAmount = product.ProductPrice,
                            CreatedBy = "demo_seeder",
                            CreatedDate = saleDate
                        });
                        sale.TotalAmount = sale.SaleDetails.Sum(d => d.TotalAmount);
                        sale.FinalAmount = sale.TotalAmount;

                        await _unitOfWork.GetRepository<Sale>().AddAsync(sale);

                        // Also create Order record for AI Staff Performance
                        var order = new Order {
                            OrderDate = saleDate,
                            OrderTime = saleDate.ToString("HH:mm"),
                            Total = sale.TotalAmount,
                            OrderStatus = "Paid",
                            OrderType = "DineIn",
                            StaffID = waiter.StaffID,
                            BranchID = branchId,
                            CreatedBy = "demo_seeder",
                            CreatedDate = saleDate
                        };
                        await _unitOfWork.GetRepository<Order>().AddAsync(order);
                    }
                }

                // 5. Seed Stock Transfers
                var transfer = new StockTransfer
                {
                    FromBranchID = 1,
                    ToBranchID = londonBranch.BranchID,
                    TransferDate = DateTime.UtcNow.AddDays(-1),
                    Status = TransferStatus.Shipped,
                    TransferNumber = "TRF-DEMO-001",
                    Remarks = "Demo: Inter-city replenishment",
                    CreatedBy = "demo_seeder",
                    Details = new List<StockTransferDetail> {
                        new StockTransferDetail { IngredientID = 1, Quantity = 10, UnitID = 1, CreatedBy = "demo_seeder" }
                    }
                };
                await _unitOfWork.GetRepository<StockTransfer>().AddAsync(transfer);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ResponseDto<bool>.CreateSuccessResponse(true, "Demo data ecosystem deployed.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ResponseDto<bool>.CreateErrorResponse(ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> ResetAdminPasswordAsync()
        {
            try
            {
                var userRepo = _unitOfWork.GetRepository<User>();
                var admin = (await userRepo.GetAllAsync()).FirstOrDefault(u => u.UserName == "admin");
                
                if (admin != null)
                {
                    admin.PasswordHash = "Yy8eXlyYGaz5Mg6Zvd1nPfIYqYNZZKXGB2sxltdw0mA";
                    admin.PasswordSalt = "7xe7O82HP8rVPMyNod2zpg";
                    admin.Status = true;
                    admin.IsDeleted = false;
                    await _unitOfWork.SaveChangesAsync();
                    return ResponseDto<bool>.CreateSuccessResponse(true, "Admin password reset to 'admin'.");
                }
                return ResponseDto<bool>.CreateErrorResponse("Admin user not found.");
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.CreateErrorResponse(ex.Message);
            }
        }
    }
}
