using RMS.Application.DTOs;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface ISystemService
    {
        Task<ResponseDto<SystemStatusDto>> GetSystemStatusAsync();
        Task<ResponseDto<bool>> InitializeSystemAsync(SystemInitializationDto initializationDto);
        Task<ResponseDto<bool>> SeedDemoDataAsync();
        Task<ResponseDto<bool>> ResetAdminPasswordAsync();
    }
}
