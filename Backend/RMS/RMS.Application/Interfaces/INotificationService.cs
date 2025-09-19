using System.Threading.Tasks;
using RMS.Application.DTOs.RealtimeUpdates; // Added for DTOs

namespace RMS.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendInventoryUpdateAsync(InventoryUpdateDto updateDto);
        Task SendOrderUpdateAsync(OrderUpdateDto updateDto);
        Task SendKitchenOrderUpdateAsync(OrderUpdateDto updateDto);
        // Add other notification methods as needed
    }
}
