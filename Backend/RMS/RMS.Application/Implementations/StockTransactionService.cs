using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.StockTransactionDTOs.InputDTOs;
using RMS.Application.DTOs.StockTransactionDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Infrastructure.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Extensions;
using System.Linq;
using System;

namespace RMS.Application.Implementations
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly IStockTransactionRepository _stockTransactionRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository; // Keep this if product details are needed elsewhere
        private readonly IMapper _mapper;
        private readonly IValidator<CreateStockTransactionDto> _createValidator;
        private readonly IValidator<UpdateStockTransactionDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StockTransactionService> _logger;
        private readonly IAlertService _alertService;

        public StockTransactionService(
            IStockTransactionRepository stockTransactionRepository,
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository,
            IMapper mapper,
            IValidator<CreateStockTransactionDto> createValidator,
            IValidator<UpdateStockTransactionDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<StockTransactionService> logger,
            IAlertService alertService)
        {
            _stockTransactionRepository = stockTransactionRepository;
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository; // Keep this if product details are needed elsewhere
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _alertService = alertService;
        }

        public async Task<ResponseDto<StockTransactionDto>> GetByIdAsync(int id)
        {
            var query = _stockTransactionRepository.GetQueryable();
            var stockTransaction = await query.Include(st => st.Product)
                                            .Include(st => st.Supplier)
                                            .FirstOrDefaultAsync(st => st.TransactionID == id);

            if (stockTransaction == null)
            {
                return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Stock Transaction not found.", Code = "404" };
            }
            var stockTransactionDto = _mapper.Map<StockTransactionDto>(stockTransaction);
            return new ResponseDto<StockTransactionDto> { IsSuccess = true, Data = stockTransactionDto, Code = "200" };
        }

        public async Task<PagedResult<StockTransactionDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _stockTransactionRepository.GetQueryable();

            query = query.Include(st => st.Product)
                         .Include(st => st.Supplier);

            if (status.HasValue)
            {
                query = query.Where(st => st.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(st => st.TransactionType.Contains(searchQuery) || st.Remarks.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.ApplySort(sortColumn, sortDirection ?? "asc");
            }
            else
            {
                query = query.OrderBy(st => st.TransactionDate); // Default sort
            }

            var pagedResult = await _stockTransactionRepository.GetPagedResultAsync(new PagedQuery { PageNumber = pageNumber, PageSize = pageSize }, null, false, query);
            var stockTransactionDtos = _mapper.Map<List<StockTransactionDto>>(pagedResult.Items);
            return new PagedResult<StockTransactionDto>(stockTransactionDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
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

                // Fetch inventory to get MinStockLevel and Product details for logging
                var inventory = await _inventoryRepository.GetByProductIdAsync(updateDto.ProductID.Value);
                if (inventory == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Inventory for product not found.", Code = "404" };
                }

                int stockChange = 0;

                // Calculate the net change from the original transaction to the updated one
                // Revert original transaction's effect
                if (existingStockTransaction.TransactionType.ToUpper() == "IN")
                {
                    stockChange -= existingStockTransaction.Quantity;
                }
                else if (existingStockTransaction.TransactionType.ToUpper() == "OUT")
                {
                    stockChange += existingStockTransaction.Quantity;
                }

                // Apply new transaction's effect
                if (updateDto.TransactionType.ToUpper() == "IN")
                {
                    stockChange += updateDto.Quantity;
                }
                else if (updateDto.TransactionType.ToUpper() == "OUT")
                {
                    stockChange -= updateDto.Quantity;
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Invalid Transaction Type. Use 'IN' or 'OUT'.", Code = "400" };
                }

                // Update inventory stock
                inventory.CurrentStock += stockChange;
                await _inventoryRepository.UpdateAsync(inventory);

                // Update the stock transaction record
                _mapper.Map(updateDto, existingStockTransaction);
                existingStockTransaction.TransactionDate = DateTime.UtcNow; // Update transaction date
                await _stockTransactionRepository.UpdateAsync(existingStockTransaction);

                await _unitOfWork.CommitTransactionAsync();

                // After successful commit, check for low stock and log alert if 'OUT' transaction
                if (updateDto.TransactionType.ToUpper() == "OUT")
                {
                    if (inventory.CurrentStock < inventory.MinStockLevel)
                    {
                        await _alertService.CreateAlertAsync(new DTOs.AlertDTOs.CreateAlertDto
                        {
                            Message = $"Stock for product {inventory.Product.ProductName} is low.",
                            Type = Domain.Enum.AlertType.LowStock
                        });
                    }
                }

                var stockTransactionDto = _mapper.Map<StockTransactionDto>(existingStockTransaction);
                await _auditLogService.LogAsync("UpdateStockTransaction", "StockTransaction", $"TransactionId:{existingStockTransaction.TransactionID}", "System", $"Stock Transaction for product '{existingStockTransaction.Product?.ProductName}' updated. Type: {existingStockTransaction.TransactionType}, Quantity: {existingStockTransaction.Quantity}.");

                return new ResponseDto<StockTransactionDto> { IsSuccess = true, Message = "Stock Transaction updated successfully.", Data = stockTransactionDto, Code = "200" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock transaction for TransactionID: {TransactionID}", updateDto.TransactionID);
                await _unitOfWork.RollbackTransactionAsync();
                return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "An error occurred while updating the stock transaction.", Code = "500" };
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

            await _auditLogService.LogAsync("DeleteStockTransaction", "StockTransaction", $"TransactionId:{id}", "System", $"Stock Transaction for product '{stockTransaction.Product?.ProductName}' deleted.");

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

            await _auditLogService.LogAsync("UpdateStockTransactionStatus", "StockTransaction", $"TransactionId:{id}", "System", $"Stock Transaction for product '{stockTransaction.Product?.ProductName}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Stock Transaction status updated successfully.", Code = "200" };
        }
        public async Task<ResponseDto<StockTransactionDto>> ProcessStockTransactionAsync(CreateStockTransactionDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errorDetails = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                _logger.LogError("Stock Transaction processing validation failed. Details: {ErrorDetails}", System.Text.Json.JsonSerializer.Serialize(errorDetails));
                return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = errorDetails };
            }

            // Start a database transaction
            await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            try
            {
                // Fetch the product's inventory to get MinStockLevel and ProductID for the transaction record
                var inventory = await _inventoryRepository.GetByProductIdAsync(createDto.ProductID.Value);

                if (inventory == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Inventory for product not found.", Code = "404" };
                }

                // Handle stock adjustments
                if (!string.IsNullOrEmpty(createDto.AdjustmentType))
                {
                    if (createDto.AdjustmentType.ToUpper() == "ADDITION")
                    {
                        inventory.CurrentStock += createDto.Quantity;
                    }
                    else if (createDto.AdjustmentType.ToUpper() == "SUBTRACTION")
                    {
                        if (inventory.CurrentStock < createDto.Quantity)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Insufficient stock for this adjustment.", Code = "400" };
                        }
                        inventory.CurrentStock -= createDto.Quantity;
                    }
                    else
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Invalid AdjustmentType. Must be 'Addition' or 'Subtraction'.", Code = "400" };
                    }
                }
                else // Regular IN/OUT transactions
                {
                    if (createDto.TransactionType.ToUpper() == "IN")
                    {
                        inventory.CurrentStock += createDto.Quantity;
                    }
                    else if (createDto.TransactionType.ToUpper() == "OUT")
                    {
                        if (inventory.CurrentStock >= createDto.Quantity)
                        {
                            inventory.CurrentStock -= createDto.Quantity;
                        }
                        else
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Insufficient stock for this transaction.", Code = "400" };
                        }
                    }
                    else
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "Invalid Transaction Type. Use 'IN', 'OUT', 'ADDITION', or 'SUBTRACTION'.", Code = "400" };
                    }
                }

                await _inventoryRepository.UpdateAsync(inventory);

                // Create and save the stock transaction record
                var stockTransaction = _mapper.Map<StockTransaction>(createDto);
                stockTransaction.TransactionDate = DateTime.UtcNow; // Ensure transaction date is set
                // If this is an ingredient consumption, ProductID in StockTransaction should be the IngredientID
                if (createDto.IngredientID.HasValue)
                {
                    stockTransaction.ProductID = createDto.IngredientID.Value;
                }
                else
                {
                    stockTransaction.ProductID = createDto.ProductID;
                }

                await _stockTransactionRepository.AddAsync(stockTransaction);

                // Commit the transaction
                await _unitOfWork.CommitTransactionAsync();

                // After successful commit, check for low stock and log alert if 'OUT' transaction
                if (createDto.TransactionType.ToUpper() == "OUT" || createDto.AdjustmentType?.ToUpper() == "SUBTRACTION")
                {
                    if (inventory.CurrentStock < inventory.MinStockLevel)
                    {
                        await _alertService.CreateAlertAsync(new DTOs.AlertDTOs.CreateAlertDto
                        {
                            Message = $"Stock for product {inventory.Product.ProductName} is low.",
                            Type = Domain.Enum.AlertType.LowStock
                        });
                    }
                }

                var stockTransactionDto = _mapper.Map<StockTransactionDto>(stockTransaction);
                await _auditLogService.LogAsync("ProcessStockTransaction", "StockTransaction", $"TransactionId:{stockTransaction.TransactionID}", "System", $"Stock Transaction for product '{stockTransaction.Product?.ProductName}' processed. Type: {stockTransaction.TransactionType}, Quantity: {stockTransaction.Quantity}. AdjustmentType: {stockTransaction.AdjustmentType}, Reason: {stockTransaction.Reason}. IngredientID: {stockTransaction.IngredientID}.");

                return new ResponseDto<StockTransactionDto> { IsSuccess = true, Message = "Stock Transaction processed successfully.", Data = stockTransactionDto, Code = "200" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing stock transaction for ProductID: {ProductID}", createDto.ProductID);
                await _unitOfWork.RollbackTransactionAsync();
                return new ResponseDto<StockTransactionDto> { IsSuccess = false, Message = "An error occurred while processing the stock transaction.", Code = "500" };
            }
        }
    }
}
