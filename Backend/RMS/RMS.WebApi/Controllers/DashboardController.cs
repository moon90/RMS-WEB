using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ITenantService _tenantService;

        public DashboardController(IDashboardService dashboardService, ITenantService tenantService)
        {
            _dashboardService = dashboardService;
            _tenantService = tenantService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var response = await _dashboardService.GetDashboardStatsAsync(_tenantService.BranchID);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                
                // Log the service-level error
                System.Console.WriteLine($"Dashboard Service Error: {response.Message}");
                
                return BadRequest(new { 
                    IsSuccess = false, 
                    Message = response.Message, 
                    Code = response.Code,
                    Details = response.Details // This now contains ex.Message if caught in service
                });
            }
            catch (Exception ex)
            {
                var details = ex.Message;
                if (ex.InnerException != null) details += " | Inner: " + ex.InnerException.Message;
                
                return StatusCode(500, new { 
                    IsSuccess = false, 
                    Message = "Critical Controller Error", 
                    Details = details,
                    StackTrace = ex.StackTrace 
                });
            }
        }
    }
}
