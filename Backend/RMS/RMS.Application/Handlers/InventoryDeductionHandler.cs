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
using System.Linq;

namespace RMS.Application.Handlers
{
    public class InventoryDeductionHandler : IConsumer<OrderPlacedEvent>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<InventoryDeductionHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public InventoryDeductionHandler(
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository,
            IProductService productService,
            INotificationService notificationService,
            ILogger<InventoryDeductionHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _productService = productService;
            _notificationService = notificationService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var domainEvent = context.Message;
            _logger.LogInformation($"Handling inventory deduction for Order: {domainEvent.Order.OrderID}");
            
            // Explicit Idempotency Check (Phase 4.4 Finalization)
            var messageId = context.MessageId?.ToString();
            if (messageId != null)
            {
                var auditRepo = _unitOfWork.GetRepository<RMS.Domain.Entities.AuditLog>();
                var alreadyProcessed = await auditRepo.GetQueryable().AnyAsync(a => a.EntityType == "Event" && a.EntityId == messageId);
                if (alreadyProcessed) 
                {
                    _logger.LogInformation($"Message {messageId} already processed. Skipping duplicate inventory deduction.");
                    return;
                }
                
                await auditRepo.AddAsync(new RMS.Domain.Entities.AuditLog 
                { 
                    Action = "Consume_OrderPlaced", 
                    EntityType = "Event", 
                    EntityId = messageId, 
                    PerformedBy = "System",
                    Details = $"Order: {domainEvent.Order.OrderID}"
                });
            }
            
            var inventoryUpdates = new List<InventoryUpdateDto>();

            try
            {
                var productIds = domainEvent.Order.OrderDetails.Select(d => d.ProductID).ToList();
                var inventories = await _inventoryRepository.GetQueryable()
                    .Where(i => productIds.Contains(i.ProductID))
                    .ToListAsync();

                foreach (var detail in domainEvent.Order.OrderDetails)
                {
                    var inventory = inventories.FirstOrDefault(i => i.ProductID == detail.ProductID);
                    if (inventory != null)
                    {
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
                            Message = $"Stock decreased for {product?.ProductName}"
                        });
                        
                        var consumeResult = await _productService.ConsumeIngredientsForProductAsync(detail.ProductID, detail.Quantity);
                        if (!consumeResult.IsSuccess)
                        {
                            _logger.LogWarning($"Ingredient consumption failed for product {detail.ProductID}: {consumeResult.Message}");
                            throw new Exception(consumeResult.Message);
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                foreach (var update in inventoryUpdates)
                {
                    await _notificationService.SendInventoryUpdateAsync(update);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process inventory deduction for Order: {domainEvent.Order.OrderID}");
                throw;
            }
        }
    }
}
