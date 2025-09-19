using RMS.Application.DTOs.AlertDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IAlertService
    {
        Task<AlertDto> CreateAlertAsync(CreateAlertDto dto);
        Task<IEnumerable<AlertDto>> GetAlertsAsync();
        Task MarkAsAcknowledgedAsync(int id);
    }
}
