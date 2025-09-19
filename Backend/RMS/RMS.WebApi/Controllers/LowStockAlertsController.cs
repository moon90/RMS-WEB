
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LowStockAlertsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public LowStockAlertsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        [Authorize(Policy = "LOW_STOCK_ALERT_VIEW")]
        public async Task<IActionResult> GetLowStockAlerts()
        {
            var alerts = await _auditLogService.GetAuditLogsByTypeAsync("LowStockAlert");
            return Ok(new ResponseDto<object>
            {
                IsSuccess = true,
                Message = "Low stock alerts retrieved successfully",
                Code = "200",
                Data = alerts
            });
        }
    }
}
