using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.Orders;
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
using RMS.Core.Enum;
using RMS.Application.DTOs.RealtimeUpdates;
using RMS.Application.Events;
using System;
using System.Linq;

namespace RMS.Application.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOrderDto> _createValidator;
        private readonly IValidator<UpdateOrderDto> _updateValidator;
        private readonly IValidator<ProcessPaymentForOrderDto> _processPaymentValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<OrderService> _logger;
        private readonly INotificationService _notificationService;
        private readonly ISplitPaymentService _splitPaymentService;
        private readonly IPurchaseService _purchaseService;
        private readonly IProductService _productService;
        private readonly IDiningTableRepository _diningTableRepository;
        private readonly IEventPublisher _eventPublisher;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            ISaleRepository saleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateOrderDto> createValidator,
            IValidator<UpdateOrderDto> updateValidator,
            IValidator<ProcessPaymentForOrderDto> processPaymentValidator,
            IAuditLogService auditLogService,
            ILogger<OrderService> logger,
            INotificationService notificationService,
            ISplitPaymentService splitPaymentService,
            IPurchaseService purchaseService,
            IProductService productService,
            IDiningTableRepository diningTableRepository,
            IEventPublisher eventPublisher)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _saleRepository = saleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _processPaymentValidator = processPaymentValidator;
            _auditLogService = auditLogService;
            _logger = logger;
            _notificationService = notificationService;
            _splitPaymentService = splitPaymentService;
            _purchaseService = purchaseService;
            _productService = productService;
            _diningTableRepository = diningTableRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<ResponseDto<OrderDto>> GetOrderByIdAsync(int id)
        {
            var orderResult = await _orderRepository.GetQueryable()
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Inventory)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (orderResult == null)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Order not found.", ApiErrorCode.NotFound);
            }
            return ResponseDto<OrderDto>.CreateSuccessResponse(_mapper.Map<OrderDto>(orderResult));
        }

        public async Task<ResponseDto<PagedResult<OrderDto>>> GetAllOrdersAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, string? status)
        {
            try {
                IQueryable<Order> query = _orderRepository.GetQueryable()
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product);

                if (!string.IsNullOrEmpty(status))
                {
                    var statusList = status.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                    if (statusList.Count > 1)
                    {
                        query = query.Where(o => statusList.Contains(o.OrderStatus));
                    }
                    else
                    {
                        var singleStatus = statusList.FirstOrDefault();
                        query = query.Where(o => o.OrderStatus == singleStatus);
                    }
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(o => 
                        (o.TableName != null && o.TableName.Contains(searchQuery)) || 
                        (o.WaiterName != null && o.WaiterName.Contains(searchQuery)) ||
                        (o.TokenNumber != null && o.TokenNumber.Contains(searchQuery)));
                }

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    query = query.ApplySort(sortColumn, sortDirection ?? "asc");
                }
                else
                {
                    query = query.OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.OrderID);
                }

                var pagedResult = await query.ToPagedList(pageNumber, pageSize);
                var orderDtos = _mapper.Map<List<OrderDto>>(pagedResult.Items);

                var result = new PagedResult<OrderDto>(orderDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
                return ResponseDto<PagedResult<OrderDto>>.CreateSuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Order fetch protocol failed.");
                return ResponseDto<PagedResult<OrderDto>>.CreateErrorResponse($"Orders Unavailable: {ex.Message}");
            }
        }

        public async Task<ResponseDto<OrderDto>> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var validationResult = await _createValidator.ValidateAsync(orderDto);
            if (!validationResult.IsValid)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var order = _mapper.Map<Order>(orderDto);
                order.TokenNumber = $"#{new Random().Next(100, 999)}";
                order.TableName ??= "N/A";
                order.WaiterName ??= "N/A";
                order.StaffID = orderDto.StaffID;
                order.OrderTime ??= DateTime.UtcNow.ToString("HH:mm");
                order.OrderStatus ??= "Pending";
                order.OrderType ??= "TakeOut";
                order.OrderDate ??= DateTime.UtcNow;

                var createdOrder = await _orderRepository.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                // Dispatch Domain Event for Inventory Deduction
                await _eventPublisher.PublishAsync(new OrderPlacedEvent(createdOrder));

                // Update Table Status for DineIn
                if (createdOrder.OrderType == "DineIn" && !string.IsNullOrEmpty(createdOrder.TableName) && createdOrder.TableName != "N/A")
                {
                    var table = await _diningTableRepository.GetQueryable().FirstOrDefaultAsync(t => t.TableName == createdOrder.TableName);
                    if (table != null)
                    {
                        table.DiningTableStatus = RMS.Domain.Enum.DiningTableStatusEnum.Occupied;
                        await _diningTableRepository.UpdateAsync(table);
                    }
                }

                await _unitOfWork.CommitTransactionAsync();
                
                // Trigger AI Auto-PO Check (Async)
                _ = Task.Run(() => _purchaseService.CheckAndGenerateAutoPOsAsync());

                await _auditLogService.LogAsync("CreateOrder", "Order", $"OrderId:{createdOrder.OrderID}", "System", $"Order created for table '{createdOrder.TableName}'.");

                var orderUpdateDto = new OrderUpdateDto
                {
                    OrderId = createdOrder.OrderID,
                    TableName = createdOrder.TableName,
                    OrderStatus = createdOrder.OrderStatus,
                    Message = $"New order created: {createdOrder.OrderID}",
                    OrderType = createdOrder.OrderType
                };
                await _notificationService.SendOrderUpdateAsync(orderUpdateDto);
                await _notificationService.SendKitchenOrderUpdateAsync(orderUpdateDto);

                return ResponseDto<OrderDto>.CreateSuccessResponse(_mapper.Map<OrderDto>(createdOrder), "Order created successfully.", "201");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Order creation failed.");
                return ResponseDto<OrderDto>.CreateErrorResponse($"Critical Order Error: {ex.Message}", ApiErrorCode.ServerError);
            }
        }

        public async Task<ResponseDto<OrderDto>> UpdateOrderAsync(int id, UpdateOrderDto orderDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(orderDto);
            if (!validationResult.IsValid)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var existingOrder = await _orderRepository.GetQueryable().Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderID == id);
            if (existingOrder == null)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Order not found.", ApiErrorCode.NotFound);
            }
            
            // Conflict Handling: If the client provided a LastModified timestamp, verify it's up to date
            if (orderDto.LastModified.HasValue && existingOrder.ModifiedDate.HasValue)
            {
                // We truncate both to seconds because JS ISO strings might lose precision vs DB DateTime2
                var clientTime = orderDto.LastModified.Value.AddTicks(-(orderDto.LastModified.Value.Ticks % TimeSpan.TicksPerSecond));
                var serverTime = existingOrder.ModifiedDate.Value.AddTicks(-(existingOrder.ModifiedDate.Value.Ticks % TimeSpan.TicksPerSecond));
                
                if (serverTime > clientTime)
                {
                    return ResponseDto<OrderDto>.CreateErrorResponse("Conflict: This order was modified by someone else since you last synced. Please refresh.", ApiErrorCode.BadRequest);
                }
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 1. Revert Old Stock
                await RevertOrderStockAsync(existingOrder);

                // 2. Update Order Data
                _mapper.Map(orderDto, existingOrder);
                await _orderRepository.UpdateAsync(existingOrder);
                await _unitOfWork.SaveChangesAsync();

                var inventoryUpdates = new List<InventoryUpdateDto>();

                // 3. Deduct New Stock
                foreach (var detail in existingOrder.OrderDetails)
                {
                    var inventory = await _inventoryRepository.GetByProductIdAsync(detail.ProductID);
                    if (inventory != null)
                    {
                        if (inventory.CurrentStock < detail.Quantity)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return ResponseDto<OrderDto>.CreateErrorResponse($"Insufficient stock for {detail.ProductID}.", ApiErrorCode.BadRequest);
                        }
                        var oldStock = inventory.CurrentStock;
                        inventory.CurrentStock -= detail.Quantity;
                        await _inventoryRepository.UpdateAsync(inventory);

                        var product = await _productRepository.GetByIdAsync(detail.ProductID);
                        inventoryUpdates.Add(new InventoryUpdateDto
                        {
                            ProductId = detail.ProductID,
                            ProductName = product?.ProductName ?? "Unknown Product",
                            OldQuantity = oldStock,
                            NewQuantity = inventory.CurrentStock,
                            ChangeType = "Sold",
                            Message = $"Stock updated for {product?.ProductName}"
                        });
                        
                        var consumeResult = await _productService.ConsumeIngredientsForProductAsync(detail.ProductID, detail.Quantity);
                        if (!consumeResult.IsSuccess)
                        {
                            // Rolling back here is essential because ConsumeIngredientsForProductAsync might have rolled back its own nested transaction
                            // but our UnitOfWork logic handles depth. However, if it failed, we must abort.
                            if (!consumeResult.IsSuccess) throw new Exception(consumeResult.Message);
                        }
                    }
                }

                // Update Table Status for DineIn (in case table changed)
                if (existingOrder.OrderType == "DineIn" && !string.IsNullOrEmpty(existingOrder.TableName) && existingOrder.TableName != "N/A")
                {
                    var table = await _diningTableRepository.GetQueryable().FirstOrDefaultAsync(t => t.TableName == existingOrder.TableName);
                    if (table != null)
                    {
                        table.DiningTableStatus = RMS.Domain.Enum.DiningTableStatusEnum.Occupied;
                        await _diningTableRepository.UpdateAsync(table);
                    }
                }

                await _unitOfWork.CommitTransactionAsync();

                // Send inventory updates after successful commit
                foreach (var update in inventoryUpdates)
                {
                    await _notificationService.SendInventoryUpdateAsync(update);
                }

                var orderUpdateDto = new OrderUpdateDto
                {
                    OrderId = existingOrder.OrderID,
                    TableName = existingOrder.TableName,
                    OrderStatus = existingOrder.OrderStatus,
                    Message = $"Order {existingOrder.OrderID} updated and inventory adjusted.",
                    OrderType = existingOrder.OrderType
                };
                await _notificationService.SendOrderUpdateAsync(orderUpdateDto);
                await _notificationService.SendKitchenOrderUpdateAsync(orderUpdateDto);

                return ResponseDto<OrderDto>.CreateSuccessResponse(_mapper.Map<OrderDto>(existingOrder), "Order updated and inventory adjusted successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to update order and adjust inventory.");
                return ResponseDto<OrderDto>.CreateErrorResponse(ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetQueryable().Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderID == id);
            if (order == null)
            {
                return ResponseDto<bool>.CreateErrorResponse("Order not found.", ApiErrorCode.NotFound);
            }

            // Safety Check: Prevent deleting already settled orders to maintain financial integrity
            if (order.PaymentStatus == "Paid")
            {
                return ResponseDto<bool>.CreateErrorResponse("Cannot delete a settled (Paid) order. Please void the associated sale first or use the 'Refund' process.", ApiErrorCode.BadRequest);
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Revert stock before deleting
                var inventoryUpdates = await RevertOrderStockAsync(order);

                // Update Table Status for DineIn (Make available)
                if (order.OrderType == "DineIn" && !string.IsNullOrEmpty(order.TableName) && order.TableName != "N/A")
                {
                    var table = await _diningTableRepository.GetQueryable().FirstOrDefaultAsync(t => t.TableName == order.TableName);
                    if (table != null)
                    {
                        table.DiningTableStatus = RMS.Domain.Enum.DiningTableStatusEnum.Available;
                        await _diningTableRepository.UpdateAsync(table);
                    }
                }

                await _orderRepository.DeleteAsync(order);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Send inventory updates after successful commit
                foreach (var update in inventoryUpdates)
                {
                    await _notificationService.SendInventoryUpdateAsync(update);
                }

                var orderUpdateDto = new OrderUpdateDto
                {
                    OrderId = order.OrderID,
                    TableName = order.TableName,
                    OrderStatus = "Deleted",
                    Message = $"Order {id} deleted and stock reverted.",
                    OrderType = order.OrderType
                };
                await _notificationService.SendOrderUpdateAsync(orderUpdateDto);
                await _notificationService.SendKitchenOrderUpdateAsync(orderUpdateDto);

                return ResponseDto<bool>.CreateSuccessResponse(true, "Order deleted and stock reverted successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to delete order and revert stock.");
                return ResponseDto<bool>.CreateErrorResponse(ex.Message);
            }
        }

        private async Task<List<InventoryUpdateDto>> RevertOrderStockAsync(Order order)
        {
            var updates = new List<InventoryUpdateDto>();
            foreach (var detail in order.OrderDetails)
            {
                var inventory = await _inventoryRepository.GetByProductIdAsync(detail.ProductID);
                if (inventory != null)
                {
                    var oldStock = inventory.CurrentStock;
                    inventory.CurrentStock += detail.Quantity;
                    await _inventoryRepository.UpdateAsync(inventory);

                    // Notify POS of product stock restore
                    var product = await _productRepository.GetByIdAsync(detail.ProductID);
                    updates.Add(new InventoryUpdateDto
                    {
                        ProductId = detail.ProductID,
                        ProductName = product?.ProductName ?? "Unknown Product",
                        OldQuantity = oldStock,
                        NewQuantity = inventory.CurrentStock,
                        ChangeType = "Restored",
                        Message = $"Stock restored for {product?.ProductName}"
                    });
                }
                
                // Revert Ingredient Consumption
                var revertResult = await _productService.RevertIngredientConsumptionAsync(detail.ProductID, detail.Quantity);
                if (!revertResult.IsSuccess)
                {
                    _logger.LogError("Stock Reversal Failed for Product {ProductID}: {Message}", detail.ProductID, revertResult.Message);
                    throw new Exception(revertResult.Message ?? "Ingredient reversal failed without a specific message.");
                }
            }
            return updates;
        }

        public async Task<ResponseDto<OrderDto>> ProcessPaymentForOrderAsync(ProcessPaymentForOrderDto paymentDto)
        {
            var validationResult = await _processPaymentValidator.ValidateAsync(paymentDto);
            if (!validationResult.IsValid)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var order = await _orderRepository.GetQueryable().Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderID == paymentDto.OrderID);
            if (order == null)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Order not found.", ApiErrorCode.NotFound);
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (paymentDto.IsSplit && paymentDto.SplitPayments != null)
                {
                    foreach (var sp in paymentDto.SplitPayments)
                    {
                        await _splitPaymentService.CreateSplitPaymentAsync(sp);
                    }
                }

                order.PaymentStatus = "Paid";
                // Industry Standard: Even if Paid, the order stays in the Kitchen (Pending/Preparing/Ready)
                // It only becomes "Completed" once the chef/server clears it.
                // So we NO LONGER set OrderStatus = "Paid" here for any type.
                
                order.PaymentMethod = paymentDto.PaymentMethod;
                order.AmountPaid = paymentDto.AmountPaid;
                order.ChangeAmount = paymentDto.ChangeAmount;
                order.DiscountAmount = paymentDto.DiscountAmount;
                order.TipAmount = paymentDto.TipAmount;
                order.PromotionID = paymentDto.PromotionID;

                await _orderRepository.UpdateAsync(order);
                
                // Create Sale Record
                var sale = new Sale
                {
                    SaleDate = DateTime.UtcNow,
                    CustomerID = order.CustomerID,
                    TotalAmount = order.Total,
                    TaxAmount = order.TaxAmount,
                    ServiceChargeAmount = order.ServiceChargeAmount,
                    DiscountAmount = order.DiscountAmount,
                    FinalAmount = order.Total + order.TaxAmount + order.ServiceChargeAmount - order.DiscountAmount + order.TipAmount,
                    PaymentMethod = order.PaymentMethod,
                    TokenNumber = order.TokenNumber,
                    TipAmount = order.TipAmount,
                    BranchID = order.BranchID,
                    SaleDetails = order.OrderDetails.Select(od => new SaleDetail
                    {
                        ProductID = od.ProductID,
                        Quantity = od.Quantity,
                        UnitPrice = od.Price,
                        Discount = od.DiscountPrice,
                        TotalAmount = od.Amount
                    }).ToList()
                };
                await _saleRepository.AddAsync(sale);

                // Update Table Status for DineIn (Make available after payment)
                if (order.OrderType == "DineIn" && !string.IsNullOrEmpty(order.TableName) && order.TableName != "N/A")
                {
                    var table = await _diningTableRepository.GetQueryable().FirstOrDefaultAsync(t => t.TableName == order.TableName);
                    if (table != null)
                    {
                        table.DiningTableStatus = RMS.Domain.Enum.DiningTableStatusEnum.Available;
                        await _diningTableRepository.UpdateAsync(table);
                    }
                }

                await _unitOfWork.CommitTransactionAsync();

                var orderUpdateDto = new OrderUpdateDto
                {
                    OrderId = order.OrderID,
                    TableName = order.TableName,
                    OrderStatus = order.OrderStatus,
                    Message = $"Payment processed for order {order.OrderID}.",
                    OrderType = order.OrderType
                };
                await _notificationService.SendOrderUpdateAsync(orderUpdateDto);
                await _notificationService.SendKitchenOrderUpdateAsync(orderUpdateDto);

                return ResponseDto<OrderDto>.CreateSuccessResponse(_mapper.Map<OrderDto>(order), "Payment processed successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ResponseDto<OrderDto>.CreateErrorResponse(ex.Message);
            }
        }

        public async Task<ResponseDto<OrderDto>> UpdateOrderStatusAsync(int id, string orderStatus, int? chefId = null)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null) return ResponseDto<OrderDto>.CreateErrorResponse("Order not found.", ApiErrorCode.NotFound);

                // AI Productivity Tracking
                if (orderStatus == "Preparing" && order.OrderStatus != "Preparing")
                {
                    order.PreparationStart = DateTime.UtcNow;
                    order.ChefID = chefId;
                }
                else if ((orderStatus == "Ready" || orderStatus == "Served") && order.OrderStatus == "Preparing")
                {
                    order.PreparationEnd = DateTime.UtcNow;
                    if (chefId.HasValue) order.ChefID = chefId;
                }

                order.OrderStatus = orderStatus;
                await _orderRepository.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();

                var orderUpdateDto = new OrderUpdateDto
                {
                    OrderId = order.OrderID,
                    TableName = order.TableName,
                    OrderStatus = order.OrderStatus,
                    Message = $"Order {order.OrderID} is now {orderStatus}.",
                    OrderType = order.OrderType
                };
                await _notificationService.SendOrderUpdateAsync(orderUpdateDto);
                await _notificationService.SendKitchenOrderUpdateAsync(orderUpdateDto);

                return ResponseDto<OrderDto>.CreateSuccessResponse(_mapper.Map<OrderDto>(order), "Order status updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Status update failed.");
                return ResponseDto<OrderDto>.CreateErrorResponse(ex.Message);
            }
        }
    }
}
