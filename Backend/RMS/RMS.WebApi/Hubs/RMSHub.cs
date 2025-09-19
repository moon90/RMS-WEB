using Microsoft.AspNetCore.SignalR;
using RMS.Application.DTOs.RealtimeUpdates; // Added for DTOs

namespace RMS.WebApi.Hubs
{
    public class RMSHub : Hub
    {
        // Methods that clients can call on the hub
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // Methods that the hub can call on clients (defined by interfaces later)
        public async Task SendInventoryUpdate(InventoryUpdateDto updateDto)
        {
            await Clients.All.SendAsync("InventoryUpdate", updateDto);
        }

        public async Task SendOrderUpdate(OrderUpdateDto updateDto)
        {
            await Clients.All.SendAsync("OrderUpdate", updateDto);
        }

        public async Task SendKitchenOrderUpdate(OrderUpdateDto updateDto)
        {
            await Clients.All.SendAsync("KitchenOrderUpdate", updateDto);
        }
    }
}
