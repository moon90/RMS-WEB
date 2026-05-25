using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.StockTransactionDTOs.InputDTOs;
using RMS.Application.DTOs.StockTransactionDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Extensions;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMS.Application.DTOs.RealtimeUpdates;

namespace RMS.Application.Implementations
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly IStockTransactionRepository _stockTransactionRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateStockTransactionDto> _createValidator;
        private readonly IValidator<UpdateStockTransactionDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StockTransactionService> _logger;
        private readonly IAlertService _alertService;
        private readonly INotificationService _notificationService;

        public StockTransactionService(
            IStockTransactionRepository stockTransactionRepository,
            IInventoryRepository inventoryRepository,
            IIngredientRepository ingredientRepository,
            IProductRepository productRepository,
            IMapper mapper,
            IValidator<CreateStockTransactionDto> createValidator,
            IValidator<UpdateStockTransactionDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<StockTransactionService> logger,
            IAlertService alertService,
            INotificationService notificationService)
        {
            _stockTransactionRepository = stockTransactionRepository;
            _inventoryRepository = inventoryRepository;
            _ingredientRepository = ingredientRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _alertService = alertService;
            _notificationService = notificationService;
        }

        public async Task<ResponseDto<StockTransactionDto>> GetByIdAsync(int id)
        {
            var query = _stockTransactionRepository.GetQueryable();
            var stockTransaction = await query.Include(st => st.Product)
                                            .Include(st => st.Supplier)
                                            .Include(st => st.Ingredient)
                                            .FirstOrDefaultAsync(st => st.TransactionID == id);

            if (stockTransaction == null)
            {
                return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Stock Transaction not found.", Code = "404" };
            }
            var stockTransactionDto = _mapper.Map<StockTransactionDto>(stockTransaction);
            return new ResponseDto<StockTransactionDto> { IsSuccess = true, Data = stockTransactionDto, Code = "200" };
        }

        public async Task<ResponseDto<PagedResult<StockTransactionDto>>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _stockTransactionRepository.GetQueryable();

            query = query.Include(st => st.Product)
                         .Include(st => st.Supplier)
                         .Include(st => st.Ingredient);

            if (status.HasValue)
            {
                query = query.Where(st => st.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(st => st.TransactionType.Contains(searchQuery) || (st.Remarks != null && st.Remarks.Contains(searchQuery)));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                if (sortColumn.Equals("ProductName", StringComparison.OrdinalIgnoreCase))
                {
                    if (sortDirection?.ToLower() == "desc")
                    {
                        query = query.OrderByDescending(st => st.Product != null ? st.Product.ProductName : (st.Ingredient != null ? st.Ingredient.Name : ""));
                    }
                    else
                    {
                        query = query.OrderBy(st => st.Product != null ? st.Product.ProductName : (st.Ingredient != null ? st.Ingredient.Name : ""));
                    }
                }
                else
                {
                    query = query.ApplySort(sortColumn, sortDirection ?? "asc");
                }
            }
            else
            {
                query = query.OrderByDescending(st => st.TransactionDate);
            }

            var pagedResult = await _stockTransactionRepository.GetPagedResultAsync(new PagedQuery { PageNumber = pageNumber, PageSize = pageSize }, null, false, query);
            var stockTransactionDtos = _mapper.Map<List<StockTransactionDto>>(pagedResult.Items);
            var result = new PagedResult<StockTransactionDto>(stockTransactionDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
            return ResponseDto<PagedResult<StockTransactionDto>>.CreateSuccessResponse(result, "Movements loaded successfully.");
        }

        public async Task<ResponseDto<StockTransactionDto>> CreateAsync(CreateStockTransactionDto createDto)
        {
            return await ProcessStockTransactionAsync(createDto);
        }

        public async Task<ResponseDto<StockTransactionDto>> UpdateAsync(UpdateStockTransactionDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var existingStockTransaction = await _stockTransactionRepository.GetByIdAsync(updateDto.TransactionID);
                if (existingStockTransaction == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Stock Transaction not found.", Code = "404" };
                }

                decimal stockChange = 0;

                // 1. Revert original transaction's effect
                if (existingStockTransaction.TransactionType.ToUpper() == "IN")
                    stockChange -= existingStockTransaction.Quantity;
                else if (existingStockTransaction.TransactionType.ToUpper() == "OUT")
                    stockChange += existingStockTransaction.Quantity;
                else if (existingStockTransaction.TransactionType.ToUpper() == "ADJUSTMENT")
                {
                    if (existingStockTransaction.AdjustmentType == "Addition") stockChange -= existingStockTransaction.Quantity;
                    else if (existingStockTransaction.AdjustmentType == "Subtraction") stockChange += existingStockTransaction.Quantity;
                }

                // 2. Apply new transaction's effect
                if (updateDto.TransactionType.ToUpper() == "IN")
                    stockChange += updateDto.Quantity;
                else if (updateDto.TransactionType.ToUpper() == "OUT")
                    stockChange -= updateDto.Quantity;
                else if (updateDto.TransactionType.ToUpper() == "ADJUSTMENT")
                {
                    if (updateDto.AdjustmentType == "Addition") stockChange += updateDto.Quantity;
                    else if (updateDto.AdjustmentType == "Subtraction") stockChange -= updateDto.Quantity;
                    else
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Adjustment Type ('Addition' or 'Subtraction') is required.", Code = "400" };
                    }
                }

                // 3. Branching Logic: Product vs Ingredient
                if (updateDto.IngredientID.HasValue)
                {
                    var ingredient = await _ingredientRepository.GetByIdAsync(updateDto.IngredientID.Value);
                    if (ingredient == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Ingredient not found.", Code = "404" };
                    }
                    ingredient.QuantityAvailable += stockChange;
                    await _ingredientRepository.UpdateAsync(ingredient);
                }
                else if (updateDto.ProductID.HasValue)
                {
                    var inventory = await _inventoryRepository.GetByProductIdAsync(updateDto.ProductID.Value);
                    if (inventory == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Inventory for product not found.", Code = "404" };
                    }
                    inventory.CurrentStock += (int)stockChange;
                    await _inventoryRepository.UpdateAsync(inventory);
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Either ProductID or IngredientID must be provided.", Code = "400" };
                }

                // Update the transaction record
                _mapper.Map(updateDto, existingStockTransaction);
                existingStockTransaction.TransactionDate = DateTime.UtcNow;
                await _stockTransactionRepository.UpdateAsync(existingStockTransaction);

                await _unitOfWork.CommitTransactionAsync();
                
                // Notify POS of stock change
                if (updateDto.ProductID.HasValue)
                {
                    var inventory = await _inventoryRepository.GetByProductIdAsync(updateDto.ProductID.Value);
                    var product = await _productRepository.GetByIdAsync(updateDto.ProductID.Value);
                    if (inventory != null)
                    {
                        await _notificationService.SendInventoryUpdateAsync(new InventoryUpdateDto
                        {
                            ProductId = updateDto.ProductID.Value,
                            ProductName = product?.ProductName ?? "Product",
                            NewQuantity = inventory.CurrentStock,
                            ChangeType = updateDto.TransactionType,
                            Message = $"Stock adjusted via Transaction: {updateDto.TransactionType}"
                        });
                    }
                }

                var stockTransactionDto = _mapper.Map<StockTransactionDto>(existingStockTransaction);
                return new ResponseDto<StockTransactionDto> { IsSuccess = true, Message = "Stock Transaction updated successfully.", Data = stockTransactionDto, Code = "200" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock transaction");
                await _unitOfWork.RollbackTransactionAsync();
                return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "An error occurred during update.", Code = "500" };
            }
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var stockTransaction = await _stockTransactionRepository.GetByIdAsync(id);
            if (stockTransaction == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Stock Transaction not found.", Code = "404" };
            }

            await _stockTransactionRepository.DeleteAsync(stockTransaction);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseDto<string> { IsSuccess = true, Message = "Stock Transaction deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var stockTransaction = await _stockTransactionRepository.GetByIdAsync(id);
            if (stockTransaction == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Stock Transaction not found.", Code = "404" };
            }

            stockTransaction.Status = status;
            await _stockTransactionRepository.UpdateAsync(stockTransaction);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseDto<string> { IsSuccess = true, Message = "Stock Transaction status updated successfully.", Code = "200" };
        }

        public async Task<ResponseDto<StockTransactionDto>> ProcessStockTransactionAsync(CreateStockTransactionDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            try
            {
                decimal stockChange = 0;
                
                // Determine stock impact
                if (createDto.TransactionType.ToUpper() == "IN") stockChange = createDto.Quantity;
                else if (createDto.TransactionType.ToUpper() == "OUT") stockChange = -createDto.Quantity;
                else if (createDto.TransactionType.ToUpper() == "ADJUSTMENT")
                {
                    if (createDto.AdjustmentType == "Addition") stockChange = createDto.Quantity;
                    else if (createDto.AdjustmentType == "Subtraction") stockChange = -createDto.Quantity;
                    else
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Adjustment Type required.", Code = "400" };
                    }
                }

                // Branching Logic: Product vs Ingredient
                if (createDto.IngredientID.HasValue)
                {
                    var ingredient = await _ingredientRepository.GetByIdAsync(createDto.IngredientID.Value);
                    if (ingredient == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Ingredient not found.", Code = "404" };
                    }
                    ingredient.QuantityAvailable += stockChange;
                    await _ingredientRepository.UpdateAsync(ingredient);
                }
                else if (createDto.ProductID.HasValue)
                {
                    var inventory = await _inventoryRepository.GetByProductIdAsync(createDto.ProductID.Value);
                    if (inventory == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Inventory for product not found.", Code = "404" };
                    }
                    inventory.CurrentStock += (int)stockChange;
                    await _inventoryRepository.UpdateAsync(inventory);
                }

                var stockTransaction = _mapper.Map<StockTransaction>(createDto);
                stockTransaction.TransactionDate = DateTime.UtcNow;
                await _stockTransactionRepository.AddAsync(stockTransaction);

                await _unitOfWork.CommitTransactionAsync();

                // Notify POS of stock change
                if (createDto.ProductID.HasValue)
                {
                    var inventory = await _inventoryRepository.GetByProductIdAsync(createDto.ProductID.Value);
                    var product = await _productRepository.GetByIdAsync(createDto.ProductID.Value);
                    if (inventory != null)
                    {
                        await _notificationService.SendInventoryUpdateAsync(new InventoryUpdateDto
                        {
                            ProductId = createDto.ProductID.Value,
                            ProductName = product?.ProductName ?? "Product",
                            NewQuantity = inventory.CurrentStock,
                            ChangeType = createDto.TransactionType,
                            Message = $"Stock updated via Transaction: {createDto.TransactionType}"
                        });
                    }
                }

                var stockTransactionDto = _mapper.Map<StockTransactionDto>(stockTransaction);
                return new ResponseDto<StockTransactionDto> { IsSuccess = true, Message = "Stock Transaction processed successfully.", Data = stockTransactionDto, Code = "200" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing stock transaction");
                await _unitOfWork.RollbackTransactionAsync();
                return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "An error occurred while processing transaction.", Code = "500" };
            }
        }
    }
}
