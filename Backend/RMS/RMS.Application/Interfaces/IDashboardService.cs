using System.Threading.Tasks;
using RMS.Application.DTOs;
using RMS.Application.DTOs.Dashboard;

namespace RMS.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<ResponseDto<DashboardDto>> GetDashboardStatsAsync(int? branchId = null);
    }
}
