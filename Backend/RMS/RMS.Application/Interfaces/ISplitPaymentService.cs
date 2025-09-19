using RMS.Application.DTOs.SplitPaymentDTOs;
using RMS.Domain.Entities;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface ISplitPaymentService
    {
        Task<SplitPaymentDto> CreateSplitPaymentAsync(CreateSplitPaymentDto dto);
    }
}
