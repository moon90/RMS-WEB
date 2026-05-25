
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Core.Enum;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Specification;
using RMS.Domain.Extensions;
using RMS.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Implementations
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateSaleDto> _createSaleValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<SaleService> _logger;
        private readonly IProductService _productService;
        private readonly IDistributedCache _cache;

        public SaleService(
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateSaleDto> createSaleValidator,
            IAuditLogService auditLogService,
            ILogger<SaleService> logger,
            IProductService productService,
            IDistributedCache cache)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createSaleValidator = createSaleValidator;
            _auditLogService = auditLogService;
            _logger = logger;
            _productService = productService;
            _cache = cache;
        }

        public async Task<ResponseDto<PagedResult<SaleDto>>> GetAllSalesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection)
        {
            try
            {
                var query = _saleRepository.GetQueryable();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(s => s.PaymentMethod.Contains(searchQuery) || (s.TokenNumber != null && s.TokenNumber.Contains(searchQuery)));
                }

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    query = query.ApplySort(sortColumn, sortDirection ?? "asc");
                }
                else
                {
                    query = query.OrderByDescending(s => s.SaleDate);
                }

                var pagedResult = await query.ToPagedList(pageNumber, pageSize);
                var saleDtos = _mapper.Map<List<SaleDto>>(pagedResult.Items);

                return ResponseDto<PagedResult<SaleDto>>.CreateSuccessResponse(new PagedResult<SaleDto>
                {
                    Items = saleDtos,
                    TotalRecords = pagedResult.TotalRecords,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching sales list.");
                return ResponseDto<PagedResult<SaleDto>>.CreateErrorResponse($"Failed to fetch sales: {ex.Message}", ApiErrorCode.ServerError);
            }
        }

        public async Task<ResponseDto<SaleDto>> CreateSaleAsync(CreateSaleDto saleDto)
        {
            _logger.LogInformation("Creating sale for customer: {CustomerID}, Payment: {PaymentMethod}", saleDto.CustomerID, saleDto.PaymentMethod);
            var validationResult = await _createSaleValidator.ValidateAsync(saleDto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Sale validation failed: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                return ResponseDto<SaleDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var sale = _mapper.Map<Sale>(saleDto);
                sale.CreatedOn = DateTime.UtcNow;
                sale.CreatedDate = DateTime.UtcNow;
                sale.CreatedBy = "system";
                sale.Status = true;
                
                // Set metadata for nested entities
                if (sale.SplitPayments != null)
                {
                    foreach (var sp in sale.SplitPayments)
                    {
                        sp.CreatedOn = DateTime.UtcNow;
                        sp.CreatedDate = DateTime.UtcNow;
                        sp.CreatedBy = "system";
                        sp.Status = true;
                    }
                }

                if (sale.SaleDetails != null)
                {
                    foreach (var sd in sale.SaleDetails)
                    {
                        sd.CreatedDate = DateTime.UtcNow;
                        sd.CreatedBy = "system";
                        sd.Status = true;
                    }
                }

                _logger.LogInformation("Adding sale record for {ItemCount} items.", sale.SaleDetails?.Count ?? 0);
                var newSale = await _saleRepository.AddAsync(sale);

                if (saleDto.SaleDetails != null)
                {
                    foreach (var detail in saleDto.SaleDetails)
                    {
                        _logger.LogInformation("Checking inventory for Product ID: {ProductID}", detail.ProductId);
                        var inventory = await _inventoryRepository.GetByProductIdAsync(detail.ProductId);

                        if (inventory != null)
                        {
                            if (inventory.CurrentStock < detail.Quantity)
                            {
                                _logger.LogWarning("Insufficient stock for Product ID: {ProductID}. Current: {Current}, Requested: {Requested}", detail.ProductId, inventory.CurrentStock, detail.Quantity);
                                await _unitOfWork.RollbackTransactionAsync();
                                return ResponseDto<SaleDto>.CreateErrorResponse($"Not enough stock for Product ID: {detail.ProductId}", ApiErrorCode.BadRequest);
                            }

                            _logger.LogInformation("Updating stock for Product ID: {ProductID}", detail.ProductId);
                            inventory.CurrentStock -= detail.Quantity;
                            await _inventoryRepository.UpdateAsync(inventory);

                            // Consume ingredients for the sold product
                            _logger.LogInformation("Consuming ingredients for Product ID: {ProductID}", detail.ProductId);
                            var consumeResult = await _productService.ConsumeIngredientsForProductAsync(detail.ProductId, detail.Quantity);
                            if (!consumeResult.IsSuccess)
                            {
                                _logger.LogWarning("Ingredient consumption failed for Product ID: {ProductID}. Error: {Error}", detail.ProductId, consumeResult.Message);
                                await _unitOfWork.RollbackTransactionAsync();
                                return ResponseDto<SaleDto>.CreateErrorResponse(consumeResult.Message, ApiErrorCode.BadRequest);
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Inventory record missing for Product ID: {ProductID}", detail.ProductId);
                            await _unitOfWork.RollbackTransactionAsync();
                            return ResponseDto<SaleDto>.CreateErrorResponse($"Inventory not found for Product ID: {detail.ProductId}", ApiErrorCode.NotFound);
                        }
                    }
                }

                await _unitOfWork.CommitTransactionAsync();

                // Customer Loyalty AI & Automated CRM
                if (newSale.CustomerID.HasValue)
                {
                    try 
                    {
                        var customer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(newSale.CustomerID.Value);
                        if (customer != null)
                        {
                            decimal oldSpent = customer.TotalSpent;
                            customer.TotalSpent += newSale.FinalAmount;
                            customer.LoyaltyPoints += (int)newSale.FinalAmount; 
                            customer.LastVisitDate = DateTime.UtcNow;

                            if (customer.TotalSpent >= 2000) customer.LoyaltyTier = "Gold";
                            else if (customer.TotalSpent >= 500) customer.LoyaltyTier = "Silver";
                            else customer.LoyaltyTier = "Bronze";

                            await _unitOfWork.GetRepository<Customer>().UpdateAsync(customer);
                            await _unitOfWork.SaveChangesAsync();

                            if (oldSpent < 1000 && customer.TotalSpent >= 1000)
                            {
                                var promo = new Promotion
                                {
                                    Description = $"Loyalty Reward: {customer.CustomerName}",
                                    CouponCode = $"LOYAL-{customer.CustomerID}-{DateTime.UtcNow:ssmm}",
                                    DiscountPercentage = 20,
                                    ValidFrom = DateTime.UtcNow,
                                    ValidTo = DateTime.UtcNow.AddDays(30),
                                    IsActive = true,
                                    CreatedBy = "AI_CRM_BOT",
                                    CreatedDate = DateTime.UtcNow
                                };
                                await _unitOfWork.GetRepository<Promotion>().AddAsync(promo);
                                await _unitOfWork.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Loyalty AI update failed.");
                    }
                }

                // Invalidate Dashboard Cache
                await _cache.RemoveAsync("DASHBOARD_STATS");

                await _auditLogService.LogAsync("CreateSale", "Sale", $"SaleId:{newSale.SaleID}", "System", $"Sale created for customer '{newSale.CustomerID}'.");

                return ResponseDto<SaleDto>.CreateSuccessResponse(_mapper.Map<SaleDto>(newSale), "Sale created successfully.", "201");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Sale creation failed. Error: {ErrorMessage}", ex.Message);
                return ResponseDto<SaleDto>.CreateErrorResponse($"Sale Service Error: {ex.Message}", ApiErrorCode.ServerError);
            }
        }

        public async Task<ResponseDto<SaleDto>> GetSaleAsync(int id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
            {
                return ResponseDto<SaleDto>.CreateErrorResponse("Sale not found.", ApiErrorCode.NotFound);
            }
            return ResponseDto<SaleDto>.CreateSuccessResponse(_mapper.Map<SaleDto>(sale));
        }

        public async Task<ResponseDto<SaleDto>> UpdateSaleAsync(int id, CreateSaleDto saleDto)
        {
            var existingSale = await _saleRepository.GetByIdAsync(id);
            if (existingSale == null)
            {
                return ResponseDto<SaleDto>.CreateErrorResponse("Sale not found.", ApiErrorCode.NotFound);
            }

            try
            {
                _mapper.Map(saleDto, existingSale);
                existingSale.ModifiedDate = DateTime.UtcNow;
                existingSale.ModifiedBy = "system";

                await _saleRepository.UpdateAsync(existingSale);
                
                await _cache.RemoveAsync("DASHBOARD_STATS");

                await _auditLogService.LogAsync("UpdateSale", "Sale", $"SaleId:{id}", "System", $"Sale ID {id} updated.");

                return ResponseDto<SaleDto>.CreateSuccessResponse(_mapper.Map<SaleDto>(existingSale), "Sale updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sale update failed for ID: {SaleID}", id);
                return ResponseDto<SaleDto>.CreateErrorResponse($"Update failed: {ex.Message}", ApiErrorCode.ServerError);
            }
        }

        public async Task<ResponseDto<bool>> DeleteSaleAsync(int id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
            {
                return ResponseDto<bool>.CreateErrorResponse("Sale not found.", ApiErrorCode.NotFound);
            }

            try
            {
                await _saleRepository.DeleteAsync(sale);

                await _cache.RemoveAsync("DASHBOARD_STATS");

                await _auditLogService.LogAsync("DeleteSale", "Sale", $"SaleId:{id}", "System", $"Sale ID {id} deleted.");
                return ResponseDto<bool>.CreateSuccessResponse(true, "Sale deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sale deletion failed for ID: {SaleID}", id);
                return ResponseDto<bool>.CreateErrorResponse($"Deletion failed: {ex.Message}", ApiErrorCode.ServerError);
            }
        }
    }
}
