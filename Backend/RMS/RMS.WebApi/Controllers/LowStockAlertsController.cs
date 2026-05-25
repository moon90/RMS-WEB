
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
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
        private readonly IAlertService _alertService;

        public LowStockAlertsController(IAlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpGet]
        [Authorize(Policy = "LOW_STOCK_ALERT_VIEW")]
        public async Task<IActionResult> GetLowStockAlerts()
        {
            var alerts = await _alertService.GetAlertsAsync();
            return Ok(new ResponseDto<object>
            {
                IsSuccess = true,
                Message = "Active alerts retrieved successfully",
                Code = "200",
                Data = alerts
            });
        }
    }
}
