using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertService _alertService;

        public AlertsController(IAlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlerts()
        {
            var alerts = await _alertService.GetAlertsAsync();
            return Ok(alerts);
        }

        [HttpPost("{id}/acknowledge")]
        public async Task<IActionResult> AcknowledgeAlert(int id)
        {
            await _alertService.MarkAsAcknowledgedAsync(id);
            return Ok();
        }
    }
}
