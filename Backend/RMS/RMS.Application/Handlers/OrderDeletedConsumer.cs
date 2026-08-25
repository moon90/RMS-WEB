using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.RealtimeUpdates;
using RMS.Application.Events;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.IRepositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Handlers
{
    public class OrderDeletedConsumer : IConsumer<OrderDeletedEvent>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<OrderDeletedConsumer> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public OrderDeletedConsumer(
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository,
            IProductService productService,
            INotificationService notificationService,
            ILogger<OrderDeletedConsumer> logger,
            IUnitOfWork unitOfWork)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _productService = productService;
            _notificationService = notificationService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<OrderDeletedEvent> context)
        {
            var order = context.Message.Order;
            _logger.LogInformation($"Handling stock reversal for deleted Order: {order.OrderID}");

            // Explicit Idempotency Check (Phase 4.4 Finalization)
            var messageId = context.MessageId?.ToString();
            if (messageId != null)
            {
                var auditRepo = _unitOfWork.GetRepository<RMS.Domain.Entities.AuditLog>();
                var alreadyProcessed = await auditRepo.GetQueryable().AnyAsync(a => a.EntityType == "Event" && a.EntityId == messageId);
                if (alreadyProcessed) 
                {
                    _logger.LogInformation($"Message {messageId} already processed. Skipping duplicate stock reversal.");
                    return;
                }
                
                await auditRepo.AddAsync(new RMS.Domain.Entities.AuditLog 
                { 
                    Action = "Consume_OrderDeleted", 
                    EntityType = "Event", 
                    EntityId = messageId, 
                    PerformedBy = "System",
                    Details = $"Order: {order.OrderID}"
                });
            }

            var updates = new List<InventoryUpdateDto>();
            foreach (var detail in order.OrderDetails)
            {
                var inventory = await _inventoryRepository.GetByProductIdAsync(detail.ProductID);
                if (inventory != null)
                {
                    var oldStock = inventory.CurrentStock;
                    inventory.CurrentStock += detail.Quantity;
                    await _inventoryRepository.UpdateAsync(inventory);

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

                var revertResult = await _productService.RevertIngredientConsumptionAsync(detail.ProductID, detail.Quantity);
                if (!revertResult.IsSuccess)
                {
                    _logger.LogError("Stock Reversal Failed for Product {ProductID}: {Message}", detail.ProductID, revertResult.Message);
                    throw new Exception(revertResult.Message ?? "Ingredient reversal failed.");
                }
            }

            await _unitOfWork.SaveChangesAsync();

            foreach (var update in updates)
            {
                await _notificationService.SendInventoryUpdateAsync(update);
            }
        }
    }
}
