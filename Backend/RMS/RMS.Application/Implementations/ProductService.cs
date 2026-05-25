
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.ProductDTOs.InputDTOs;
using RMS.Application.DTOs.ProductDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
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
using RMS.Application.DTOs.StockTransactionDTOs.InputDTOs;
using RMS.Application.DTOs.RealtimeUpdates; // Added for DTOs

namespace RMS.Application.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;
        private readonly IProductIngredientRepository _productIngredientRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IStockTransactionService _stockTransactionService;
        private readonly IUnitConversionService _unitConversionService;
        private readonly INotificationService _notificationService; 
        private readonly IAlertService _alertService;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper,
            IValidator<CreateProductDto> createValidator,
            IValidator<UpdateProductDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<ProductService> logger,
            IProductIngredientRepository productIngredientRepository,
            IIngredientRepository ingredientRepository,
            IInventoryRepository inventoryRepository,
            IStockTransactionService stockTransactionService,
            IUnitConversionService unitConversionService,
            INotificationService notificationService,
            IAlertService alertService) 
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _productIngredientRepository = productIngredientRepository;
            _ingredientRepository = ingredientRepository;
            _inventoryRepository = inventoryRepository;
            _stockTransactionService = stockTransactionService;
            _unitConversionService = unitConversionService;
            _notificationService = notificationService; 
            _alertService = alertService;
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(int id)
        {
            var query = _productRepository.GetQueryable();
            var product = await query.Include(p => p.Category)
                                     .Include(p => p.Supplier)
                                     .Include(p => p.Manufacturer)
                                     .Include(p => p.Inventory)
                                     .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return new ResponseDto<ProductDto> { IsSuccess = false, Message = "Product not found.", Code = "404" };
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return new ResponseDto<ProductDto> { IsSuccess = true, Data = productDto, Code = "200" };
        }

        public async Task<ResponseDto<PagedResult<ProductDto>>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status, int? categoryId, int? supplierId = null, int? manufacturerId = null)
        {
            try
            {
                var query = _productRepository.GetQueryable();

                query = query.Include(p => p.Category)
                             .Include(p => p.Supplier)
                             .Include(p => p.Manufacturer)
                             .Include(p => p.Inventory);

                if (status.HasValue)
                {
                    query = query.Where(p => p.Status == status.Value);
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryID == categoryId.Value);
                }

                if (supplierId.HasValue)
                {
                    query = query.Where(p => p.SupplierID == supplierId.Value);
                }

                if (manufacturerId.HasValue)
                {
                    query = query.Where(p => p.ManufacturerID == manufacturerId.Value);
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(p => p.ProductName.Contains(searchQuery) || p.ProductBarcode.Contains(searchQuery) || p.Id.ToString().Contains(searchQuery));
                }

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    query = query.ApplySort(sortColumn, sortDirection ?? "asc");
                }
                else
                {
                    query = query.OrderBy(p => p.ProductName); // Default sort
                }

                var pagedResult = await query.ToPagedList(pageNumber, pageSize);
                var productDtos = _mapper.Map<List<ProductDto>>(pagedResult.Items);
                var result = new PagedResult<ProductDto>(productDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);

                return new ResponseDto<PagedResult<ProductDto>>
                {
                    IsSuccess = true,
                    Message = "Products retrieved successfully.",
                    Code = "200",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products.");
                return new ResponseDto<PagedResult<ProductDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving products.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<ResponseDto<ProductDto>> CreateAsync(CreateProductDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errorDetails = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                _logger.LogError("Product creation validation failed. Details: {ErrorDetails}", System.Text.Json.JsonSerializer.Serialize(errorDetails));
                return new ResponseDto<ProductDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = errorDetails };
            }

            var product = _mapper.Map<Product>(createDto);
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            // AUTOMATIC INVENTORY INITIALIZATION
            // Create a default inventory entry for the new product to ensure visibility in stock list
            try
            {
                var inventory = new Inventory
                {
                    ProductID = product.Id,
                    InitialStock = 0,
                    CurrentStock = 0,
                    MinStockLevel = 5, // Default safe threshold
                    LastUpdated = DateTime.UtcNow,
                    Status = true,
                    BranchID = product.BranchID ?? 1 // Link to the same branch
                };
                await _inventoryRepository.AddAsync(inventory);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("System initialized default inventory protocol for Product ID: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Inventory auto-initialization failed for Product: {ProductName}", product.ProductName);
                // We don't rollback the product creation here, but we log the failure.
            }

            var productDto = _mapper.Map<ProductDto>(product);
            await _auditLogService.LogAsync("CreateProduct", "Product", $"ProductId:{product.Id}", "System", $"Product '{product.ProductName}' created.");

            return new ResponseDto<ProductDto> { IsSuccess = true, Message = "Product created successfully.", Data = productDto, Code = "201" };
        }

        public async Task<ResponseDto<ProductDto>> UpdateAsync(UpdateProductDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<ProductDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var existingProduct = await _productRepository.GetByIdAsync(updateDto.Id);
            if (existingProduct == null)
            {
                return new ResponseDto<ProductDto> { IsSuccess = false, Message = "Product not found.", Code = "404" };
            }

            _mapper.Map(updateDto, existingProduct);
            await _productRepository.UpdateAsync(existingProduct);
            await _unitOfWork.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(existingProduct);
            await _auditLogService.LogAsync("UpdateProduct", "Product", $"ProductId:{existingProduct.Id}", "System", $"Product '{existingProduct.ProductName}' updated.");

            return new ResponseDto<ProductDto> { IsSuccess = true, Message = "Product updated successfully.", Data = productDto, Code = "200" };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Product not found.", Code = "404" };
            }

            await _productRepository.DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("DeleteProduct", "Product", $"ProductId:{id}", "System", $"Product '{product.ProductName}' deleted.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Product deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Product not found.", Code = "404" };
            }

            product.Status = status;
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("UpdateProductStatus", "Product", $"ProductId:{id}", "System", $"Product '{product.ProductName}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Product status updated successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> ConsumeIngredientsForProductAsync(int productId, int quantitySold)
        {
            await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var productIngredients = await _productIngredientRepository.GetQueryable()
                                                                           .Where(pi => pi.ProductID == productId)
                                                                           .Include(pi => pi.Ingredient)
                                                                           .Include(pi => pi.Unit)
                                                                           .ToListAsync();

                if (!productIngredients.Any())
                {
                    // No ingredients defined for this product, so nothing to consume.
                    await _unitOfWork.CommitTransactionAsync();
                    return new ResponseDto<string> { IsSuccess = true, Message = "No ingredients to consume for this product.", Code = "200" };
                }

                foreach (var pi in productIngredients)
                {
                    var requiredQuantity = pi.Quantity * quantitySold;
                    var ingredient = await _ingredientRepository.GetByIdAsync(pi.IngredientID);

                    if (ingredient == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<string> { IsSuccess = false, Message = $"Ingredient with ID {pi.IngredientID} not found.", Code = "404" };
                    }

                    // Perform unit conversion if necessary
                    decimal finalQuantityToDeduct = requiredQuantity;
                    if (pi.UnitID != ingredient.UnitID)
                    {
                        var conversionResult = await _unitConversionService.ConvertUnitsAsync(pi.UnitID, ingredient.UnitID, requiredQuantity);
                        if (!conversionResult.IsSuccess)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return new ResponseDto<string> { IsSuccess = false, Message = $"Unit conversion failed for ingredient {ingredient.Name}: {conversionResult.Message}", Code = conversionResult.Code };
                        }
                        finalQuantityToDeduct = conversionResult.Data;
                    }

                    // Assuming we have the old quantity before deduction
                    var oldQuantity = ingredient.QuantityAvailable + finalQuantityToDeduct; // This is a simplification

                    if (ingredient.QuantityAvailable < finalQuantityToDeduct)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<string> { IsSuccess = false, Message = $"Insufficient quantity of {ingredient.Name} available. Required: {finalQuantityToDeduct} {ingredient.Unit?.Name}, Available: {ingredient.QuantityAvailable} {ingredient.Unit?.Name}.", Code = "400" };
                    }

                    ingredient.QuantityAvailable -= finalQuantityToDeduct;
                    await _ingredientRepository.UpdateAsync(ingredient);

                    // Check for low stock alert
                    if (ingredient.QuantityAvailable <= ingredient.ReorderLevel)
                    {
                        await _alertService.CreateAlertAsync(new DTOs.AlertDTOs.CreateAlertDto
                        {
                            Message = $"Low Stock Alert: '{ingredient.Name}' has only {ingredient.QuantityAvailable} {ingredient.Unit?.ShortCode} remaining.",
                            Type = Domain.Enum.AlertType.LowStock
                        });
                    }

                    // Log as a stock transaction (OUT type)
                    var stockTransactionDto = new CreateStockTransactionDto
                    {
                        ProductID = productId, // This is the finished product ID
                        IngredientID = pi.IngredientID, // The consumed ingredient ID
                        TransactionType = "OUT",
                        Quantity = (int)Math.Max(1, Math.Round(finalQuantityToDeduct)), // Assuming quantity is integer for stock transactions
                        TransactionDate = DateTime.UtcNow,
                        Remarks = $"Consumed for product sale (Product ID: {productId}, Quantity Sold: {quantitySold})",
                        TransactionSource = "RecipeConsumption",
                    };
                    var transactionResult = await _stockTransactionService.ProcessStockTransactionAsync(stockTransactionDto);
                    if (!transactionResult.IsSuccess)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<string> { IsSuccess = false, Message = $"Stock transaction failed: {transactionResult.Message}", Code = transactionResult.Code };
                    }

                    // Send real-time update for each ingredient consumed
                    var inventoryUpdateDto = new InventoryUpdateDto
                    {
                        ProductId = ingredient.IngredientID,
                        ProductName = ingredient.Name,
                        OldQuantity = oldQuantity,
                        NewQuantity = ingredient.QuantityAvailable,
                        ChangeType = "Consumed",
                        Message = $"Ingredient '{ingredient.Name}' consumed. Old: {oldQuantity}, New: {ingredient.QuantityAvailable}"
                    };
                    await _notificationService.SendInventoryUpdateAsync(inventoryUpdateDto);
                }

                await _unitOfWork.CommitTransactionAsync();

                return new ResponseDto<string> { IsSuccess = true, Message = "Ingredients consumed successfully.", Code = "200" };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error consuming ingredients for product {ProductId} (Quantity Sold: {QuantitySold}).", productId, quantitySold);
                return new ResponseDto<string> { IsSuccess = false, Message = $"Error: {ex.Message}", Code = "500" };
            }
        }

        public async Task<ResponseDto<string>> RevertIngredientConsumptionAsync(int productId, int quantityToReturn)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var productIngredients = await _productIngredientRepository.GetQueryable()
                                                                           .Where(pi => pi.ProductID == productId)
                                                                           .Include(pi => pi.Ingredient)
                                                                           .Include(pi => pi.Unit)
                                                                           .ToListAsync();

                if (!productIngredients.Any())
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return new ResponseDto<string> { IsSuccess = true, Message = "No ingredients to revert for this product.", Code = "200" };
                }

                foreach (var pi in productIngredients)
                {
                    var quantityToRestore = pi.Quantity * quantityToReturn;
                    var ingredient = await _ingredientRepository.GetByIdAsync(pi.IngredientID);

                    if (ingredient == null) continue;

                    // Perform unit conversion if necessary
                    decimal finalQuantityToRestore = quantityToRestore;
                    if (pi.UnitID != ingredient.UnitID)
                    {
                        var conversionResult = await _unitConversionService.ConvertUnitsAsync(pi.UnitID, ingredient.UnitID, quantityToRestore);
                        if (conversionResult.IsSuccess)
                        {
                            finalQuantityToRestore = conversionResult.Data;
                        }
                    }

                    var oldQuantity = ingredient.QuantityAvailable;
                    ingredient.QuantityAvailable += finalQuantityToRestore;
                    await _ingredientRepository.UpdateAsync(ingredient);

                    // Log as a stock transaction (IN type)
                    var stockTransactionDto = new CreateStockTransactionDto
                    {
                        ProductID = productId,
                        IngredientID = pi.IngredientID,
                        TransactionType = "IN",
                        Quantity = (int)Math.Max(1, Math.Round(finalQuantityToRestore)),
                        TransactionDate = DateTime.UtcNow,
                        Remarks = $"Reverted consumption due to order reversal (Product ID: {productId}, Quantity Reverted: {quantityToReturn})",
                        TransactionSource = "RecipeReversal",
                    };
                    var transactionResult = await _stockTransactionService.ProcessStockTransactionAsync(stockTransactionDto);
                    if (!transactionResult.IsSuccess)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return new ResponseDto<string> { IsSuccess = false, Message = $"Stock transaction failed: {transactionResult.Message}", Code = transactionResult.Code };
                    }

                    // Send real-time update
                    var inventoryUpdateDto = new InventoryUpdateDto
                    {
                        ProductId = ingredient.IngredientID,
                        ProductName = ingredient.Name,
                        OldQuantity = oldQuantity,
                        NewQuantity = ingredient.QuantityAvailable,
                        ChangeType = "Reverted",
                        Message = $"Ingredient '{ingredient.Name}' restored. Old: {oldQuantity}, New: {ingredient.QuantityAvailable}"
                    };
                    await _notificationService.SendInventoryUpdateAsync(inventoryUpdateDto);
                }

                await _unitOfWork.CommitTransactionAsync();
                return new ResponseDto<string> { IsSuccess = true, Message = "Ingredients restored successfully.", Code = "200" };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error reverting ingredients for product {ProductId}.", productId);
                return new ResponseDto<string> { IsSuccess = false, Message = $"Error: {ex.Message}", Code = "500" };
            }
        }
    }
}
