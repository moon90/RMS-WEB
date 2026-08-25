using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RMS.Application.Interfaces;
using System.Threading.Tasks;

namespace RMS.WebApi.Hubs
{
    [Authorize]
    public class ReservationHub : Hub<IReservationHubClient>
    {
        public const string HostessGroup = "HostessFloorPlan";

        public async Task JoinHostessGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, HostessGroup);
        }

        public async Task LeaveHostessGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, HostessGroup);
        }
    }
}
