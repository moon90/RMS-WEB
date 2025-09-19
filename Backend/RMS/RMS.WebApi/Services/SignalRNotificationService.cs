using Microsoft.AspNetCore.SignalR;
using RMS.Application.Interfaces;
using RMS.WebApi.Hubs;
using System.Threading.Tasks;
using RMS.Application.DTOs.RealtimeUpdates; // Added for DTOs

namespace RMS.WebApi.Services
{
    public class SignalRNotificationService : INotificationService
    {
        private readonly IHubContext<RMSHub> _hubContext;

        public SignalRNotificationService(IHubContext<RMSHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendInventoryUpdateAsync(InventoryUpdateDto updateDto)
        {
            await _hubContext.Clients.All.SendAsync("InventoryUpdate", updateDto);
        }

        public async Task SendOrderUpdateAsync(OrderUpdateDto updateDto)
        {
            await _hubContext.Clients.All.SendAsync("OrderUpdate", updateDto);
        }

        public async Task SendKitchenOrderUpdateAsync(OrderUpdateDto updateDto)
        {
            await _hubContext.Clients.All.SendAsync("KitchenOrderUpdate", updateDto);
        }
    }
}
