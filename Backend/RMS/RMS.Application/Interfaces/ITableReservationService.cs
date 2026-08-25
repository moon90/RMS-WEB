using RMS.Application.DTOs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface ITableReservationService
    {
        Task<ResponseDto<TableReservationDto>> CreateHoldAsync(CreateReservationHoldDto dto, string createdBy, CancellationToken cancellationToken = default);
        Task<ResponseDto<bool>> CancelHoldAsync(Guid reservationId, string cancelledBy, CancellationToken cancellationToken = default);
        Task<int> ReleaseExpiredHoldsAsync(CancellationToken cancellationToken = default);
    }
}
