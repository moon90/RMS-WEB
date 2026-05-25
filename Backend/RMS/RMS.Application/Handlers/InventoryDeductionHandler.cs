using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.RealtimeUpdates;
using RMS.Application.Events;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.IRepositories;

namespace RMS.Application.Handlers
{
    public class InventoryDeductionHandler : IEventHandler<OrderPlacedEvent>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<InventoryDeductionHandler> _logger;

        public InventoryDeductionHandler(
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository,
            IProductService productService,
            INotificationService notificationService,
            ILogger<InventoryDeductionHandler> logger)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _productService = productService;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task HandleAsync(OrderPlacedEvent domainEvent)
        {
            _logger.LogInformation($"Handling inventory deduction for Order: {domainEvent.Order.OrderID}");
            
            var inventoryUpdates = new List<InventoryUpdateDto>();

            try
            {
                foreach (var detail in domainEvent.Order.OrderDetails)
                {
                    var inventory = await _inventoryRepository.GetByProductIdAsync(detail.ProductID);
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
