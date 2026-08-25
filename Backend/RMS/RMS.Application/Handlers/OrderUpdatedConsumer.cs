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
    public class OrderUpdatedConsumer : IConsumer<OrderUpdatedEvent>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<OrderUpdatedConsumer> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public OrderUpdatedConsumer(
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository,
            IProductService productService,
            INotificationService notificationService,
            ILogger<OrderUpdatedConsumer> logger,
            IUnitOfWork unitOfWork)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _productService = productService;
            _notificationService = notificationService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<OrderUpdatedEvent> context)
        {
            var oldOrder = context.Message.OldOrder;
            var newOrder = context.Message.NewOrder;

            _logger.LogInformation($"Handling stock changes for updated Order: {newOrder.OrderID}");

            // Explicit Idempotency Check (Phase 4.4 Finalization)
            var messageId = context.MessageId?.ToString();
            if (messageId != null)
            {
                var auditRepo = _unitOfWork.GetRepository<RMS.Domain.Entities.AuditLog>();
                var alreadyProcessed = await auditRepo.GetQueryable().AnyAsync(a => a.EntityType == "Event" && a.EntityId == messageId);
                if (alreadyProcessed) 
                {
                    _logger.LogInformation($"Message {messageId} already processed. Skipping duplicate stock adjustment.");
                    return;
                }
                
                await auditRepo.AddAsync(new RMS.Domain.Entities.AuditLog 
                { 
                    Action = "Consume_OrderUpdated", 
                    EntityType = "Event", 
                    EntityId = messageId, 
                    PerformedBy = "System",
                    Details = $"Order: {newOrder.OrderID}"
                });
            }

            // 1. Revert Old Order Stock
            foreach (var detail in oldOrder.OrderDetails)
            {
                var inventory = await _inventoryRepository.GetByProductIdAsync(detail.ProductID);
                if (inventory != null)
                {
                    inventory.CurrentStock += detail.Quantity;
                    await _inventoryRepository.UpdateAsync(inventory);
                }
                var revertResult = await _productService.RevertIngredientConsumptionAsync(detail.ProductID, detail.Quantity);
                if (!revertResult.IsSuccess)
                {
                    throw new Exception(revertResult.Message ?? "Ingredient reversal failed.");
                }
            }

            // 2. Deduct New Order Stock
            foreach (var detail in newOrder.OrderDetails)
            {
                var inventory = await _inventoryRepository.GetByProductIdAsync(detail.ProductID);
                if (inventory != null)
                {
                    var oldStock = inventory.CurrentStock;
                    inventory.CurrentStock -= detail.Quantity;
                    await _inventoryRepository.UpdateAsync(inventory);
                    
                    var product = await _productRepository.GetByIdAsync(detail.ProductID);
                    await _notificationService.SendInventoryUpdateAsync(new InventoryUpdateDto
                    {
                        ProductId = detail.ProductID,
                        ProductName = product?.ProductName ?? "Unknown Product",
                        OldQuantity = oldStock,
                        NewQuantity = inventory.CurrentStock,
                        ChangeType = "Sold",
                        Message = $"Stock decreased for {product?.ProductName}"
                    });
                }
                var consumeResult = await _productService.ConsumeIngredientsForProductAsync(detail.ProductID, detail.Quantity);
                if (!consumeResult.IsSuccess)
                {
                    throw new Exception(consumeResult.Message);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
