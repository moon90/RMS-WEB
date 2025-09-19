using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RMS.Application.Implementations;
using RMS.Application.Interfaces;
using RMS.Application.Services.Processing;
using RMS.Application.Validators;
using RMS.Application.Validators.CustomerValidators;
using RMS.Application.Validators.DiningTableValidators;
using RMS.Application.Validators.IngredientValidators;
using RMS.Application.Validators.InventoryValidators;
using RMS.Application.Validators.ManufacturerValidators;
using RMS.Application.Validators.OrderValidators;
using RMS.Application.Validators.ProductIngredientValidators;
using RMS.Application.Validators.ProductValidators;
using RMS.Application.Validators.PurchaseValidators;
using RMS.Application.Validators.StaffValidators;
using RMS.Application.Validators.StockTransactionValidators;
using RMS.Application.Validators.SupplierValidators;
using RMS.Application.Validators.UnitValidators;
using RMS.Application.Validators.SaleValidators;
using RMS.Application.Validators.UnitConversionValidators;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.Interfaces;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;
using RMS.Infrastructure.Repositories;
using System.Reflection;

namespace RMS.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationAndInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Infrastructure Services
            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(RestaurantDbContext).Assembly.FullName)));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IRoleMenuRepository, RoleMenuRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<IProductIngredientRepository, ProductIngredientRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IDiningTableRepository, DiningTableRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IUnitConversionRepository, UnitConversionRepository>();
            services.AddScoped<ISplitPaymentRepository, SplitPaymentRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application Services
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IManufacturerService, ManufacturerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IStockTransactionService, StockTransactionService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IProductIngredientService, ProductIngredientService>();
            services.AddScoped<IDiningTableService, DiningTableService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IImageProcessingService, ImageProcessingService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IAlertService, AlertService>();
            services.AddScoped<IUnitConversionService, UnitConversionService>();
            services.AddScoped<ISplitPaymentService, SplitPaymentService>();

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<CreatePurchaseDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<MenuCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<MenuUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<RoleCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<RoleUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UserCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UserUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<ResetPasswordDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<PermissionCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<PermissionUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoryCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoryUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateUnitDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateUnitDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateSupplierDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateSupplierDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateManufacturerDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateManufacturerDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateProductDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateCustomerDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateCustomerDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateStaffDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateStaffDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateInventoryDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateInventoryDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateStockTransactionDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateStockTransactionDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateIngredientDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateIngredientDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateProductIngredientDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateProductIngredientDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateOrderDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateOrderDetailDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateOrderDetailDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<ProcessPaymentForOrderDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateDiningTableDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateDiningTableDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateDiningTableStatusDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreatePromotionDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdatePromotionDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateSaleDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateUnitConversionDtoValidator>();

            return services;
        }
    }
}
