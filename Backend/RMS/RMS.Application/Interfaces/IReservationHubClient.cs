using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IReservationHubClient
    {
        Task ReceiveReservationUpdate(object reservationPayload);
        Task ReceivePersonalConfirmation(object confirmationPayload);
    }
}
