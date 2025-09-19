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
using RMS.Application.DTOs.RealtimeUpdates; // Added for DTOs

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
        private readonly INotificationService _notificationService; // Added for SignalR
        private readonly ISplitPaymentService _splitPaymentService;

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
            ISplitPaymentService splitPaymentService) // Injected INotificationService
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
            _notificationService = notificationService; // Assigned INotificationService
            _splitPaymentService = splitPaymentService;
            _splitPaymentService = splitPaymentService;
        }

        public async Task<ResponseDto<OrderDto>> GetOrderByIdAsync(int id)
        {
            var orderResult = await _orderRepository.GetByIdAsync(id);
            if (orderResult == null)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Order not found.", ApiErrorCode.NotFound);
            }
            return ResponseDto<OrderDto>.CreateSuccessResponse(_mapper.Map<OrderDto>(orderResult));
        }

        public async Task<ResponseDto<PagedResult<OrderDto>>> GetAllOrdersAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, string? status)
        {
            var query = _orderRepository.GetQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.OrderStatus == status);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(o => o.TableName.Contains(searchQuery) || o.WaiterName.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.ApplySort(sortColumn, sortDirection ?? "asc");
            }
            else
            {
                query = query.OrderByDescending(o => o.OrderTime);
            }

            var pagedResult = await query.ToPagedList(pageNumber, pageSize);
            var orderDtos = _mapper.Map<List<OrderDto>>(pagedResult.Items);

            return ResponseDto<PagedResult<OrderDto>>.CreateSuccessResponse(new PagedResult<OrderDto>
            {
                Items = orderDtos,
                TotalRecords = pagedResult.TotalRecords,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            });
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
                var createdOrder = await _orderRepository.AddAsync(order);

                decimal totalCostOfGoodsSold = 0;

                foreach (var detail in createdOrder.OrderDetails)
                {
                    var product = await _productRepository.GetByIdAsync(detail.ProductID);
                    if (product == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ResponseDto<OrderDto>.CreateErrorResponse($"Product with ID {detail.ProductID} not found.", ApiErrorCode.NotFound);
                    }

                    var inventory = await _inventoryRepository.GetByProductIdAsync(detail.ProductID);
                    if (inventory == null || inventory.CurrentStock < detail.Quantity)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ResponseDto<OrderDto>.CreateErrorResponse($"Not enough stock for product {product.ProductName}.", ApiErrorCode.BadRequest);
                    }

                    inventory.CurrentStock -= detail.Quantity;
                    await _inventoryRepository.UpdateAsync(inventory);

                    totalCostOfGoodsSold += (product.CostPrice ?? 0) * detail.Quantity;
                }

                var sale = new Sale
                {
                    SaleDate = DateTime.UtcNow,
                    CustomerID = createdOrder.CustomerID,
                    TotalAmount = createdOrder.Total,
                    DiscountAmount = createdOrder.DiscountAmount,
                    FinalAmount = createdOrder.Total - createdOrder.DiscountAmount,
                    PaymentMethod = createdOrder.PaymentMethod,
                    SaleDetails = createdOrder.OrderDetails.Select(od => new SaleDetail
                    {
                        ProductID = od.ProductID,
                        Quantity = od.Quantity,
                        UnitPrice = od.Price,
                        Discount = od.DiscountPrice,
                        TotalAmount = od.Amount
                    }).ToList()
                };

                await _saleRepository.AddAsync(sale);

                await _unitOfWork.CommitTransactionAsync();

                await _auditLogService.LogAsync("CreateOrder", "Order", $"OrderId:{createdOrder.OrderID}", "System", $"Order created for table '{createdOrder.TableName}'.");

                var orderUpdateDto = new OrderUpdateDto
                {
                    OrderId = createdOrder.OrderID,
                    TableName = createdOrder.TableName,
                    OrderStatus = createdOrder.OrderStatus,
                    Message = $"New order created: {createdOrder.OrderID} for table {createdOrder.TableName}",
                    OrderType = createdOrder.OrderType
                };
                await _notificationService.SendOrderUpdateAsync(orderUpdateDto);
                await _notificationService.SendKitchenOrderUpdateAsync(orderUpdateDto);

                return ResponseDto<OrderDto>.CreateSuccessResponse(_mapper.Map<OrderDto>(createdOrder), "Order created successfully.", "201");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "An error occurred while creating the order.");
                return ResponseDto<OrderDto>.CreateErrorResponse("An error occurred while creating the order.", ApiErrorCode.ServerError);
            }
        }

        public async Task<ResponseDto<OrderDto>> UpdateOrderAsync(int id, UpdateOrderDto orderDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(orderDto);
            if (!validationResult.IsValid)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Order not found.", ApiErrorCode.NotFound);
            }

            _mapper.Map(orderDto, existingOrder);
            await _orderRepository.UpdateAsync(existingOrder);
            await _unitOfWork.CommitTransactionAsync();

            await _auditLogService.LogAsync("UpdateOrder", "Order", $"OrderId:{existingOrder.OrderID}", "System", $"Order for table '{existingOrder.TableName}' updated.");

            var orderUpdateDto = new OrderUpdateDto
            {
                OrderId = existingOrder.OrderID,
                TableName = existingOrder.TableName,
                OrderStatus = existingOrder.OrderStatus,
                Message = $"Order {existingOrder.OrderID} for table {existingOrder.TableName} updated.",
                OrderType = existingOrder.OrderType
            };
            await _notificationService.SendOrderUpdateAsync(orderUpdateDto);

            return ResponseDto<OrderDto>.CreateSuccessResponse(_mapper.Map<OrderDto>(existingOrder), "Order updated successfully.");
        }

        public async Task<ResponseDto<bool>> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return ResponseDto<bool>.CreateErrorResponse("Order not found.", ApiErrorCode.NotFound);
            }

            await _orderRepository.DeleteAsync(order);
            await _unitOfWork.CommitTransactionAsync();

            await _auditLogService.LogAsync("DeleteOrder", "Order", $"OrderId:{id}", "System", $"Order for table '{order.TableName}' deleted.");

            var orderUpdateDto = new OrderUpdateDto
            {
                OrderId = order.OrderID,
                TableName = order.TableName,
                OrderStatus = "Deleted", // Assuming status becomes deleted
                Message = $"Order {id} for table {order.TableName} deleted.",
                OrderType = order.OrderType
            };
            await _notificationService.SendOrderUpdateAsync(orderUpdateDto);

            return ResponseDto<bool>.CreateSuccessResponse(true, "Order deleted successfully.");
        }

        public async Task<ResponseDto<OrderDto>> ProcessPaymentForOrderAsync(ProcessPaymentForOrderDto paymentDto)
        {
            var validationResult = await _processPaymentValidator.ValidateAsync(paymentDto);
            if (!validationResult.IsValid)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var order = await _orderRepository.GetByIdAsync(paymentDto.OrderID);
            if (order == null)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Order not found.", ApiErrorCode.NotFound);
            }

            if (paymentDto.IsSplit)
            {
                foreach (var splitPayment in paymentDto.SplitPayments)
                {
                    await _splitPaymentService.CreateSplitPaymentAsync(splitPayment);
                }
            }

            order.PaymentStatus = "Paid"; // Example: Update payment status
            order.PaymentMethod = paymentDto.PaymentMethod;
            order.AmountPaid = paymentDto.AmountPaid;
            order.ChangeAmount = paymentDto.ChangeAmount;

            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.CommitTransactionAsync();

            await _auditLogService.LogAsync("ProcessPayment", "Order", $"OrderId:{order.OrderID}", "System", $"Payment processed for order '{order.OrderID}'.");

            var orderUpdateDto = new OrderUpdateDto
            {
                OrderId = order.OrderID,
                TableName = order.TableName,
                OrderStatus = order.OrderStatus,
                Message = $"Payment processed for order {order.OrderID}.",
                OrderType = order.OrderType
            };
            await _notificationService.SendOrderUpdateAsync(orderUpdateDto);

            return ResponseDto<OrderDto>.CreateSuccessResponse(_mapper.Map<OrderDto>(order), "Payment processed successfully.");
        }
        public async Task<ResponseDto<OrderDto>> UpdateOrderStatusAsync(int id, string orderStatus)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return ResponseDto<OrderDto>.CreateErrorResponse("Order not found.", ApiErrorCode.NotFound);
            }

            order.OrderStatus = orderStatus;
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.CommitTransactionAsync();

            await _auditLogService.LogAsync("UpdateOrderStatus", "Order", $"OrderId:{order.OrderID}", "System", $"Order status updated to {orderStatus} for order '{order.OrderID}'.");

            var orderUpdateDto = new OrderUpdateDto
            {
                OrderId = order.OrderID,
                TableName = order.TableName,
                OrderStatus = order.OrderStatus,
                Message = $"Order {order.OrderID} status updated to {order.OrderStatus}.",
                OrderType = order.OrderType
            };
            await _notificationService.SendOrderUpdateAsync(orderUpdateDto);

            return ResponseDto<OrderDto>.CreateSuccessResponse(_mapper.Map<OrderDto>(order), "Order status updated successfully.");
        }

    }
}

