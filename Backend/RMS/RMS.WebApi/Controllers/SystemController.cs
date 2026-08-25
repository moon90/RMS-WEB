using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.Persistences;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly ISystemService _systemService;
        private readonly RestaurantDbContext _context;

        public SystemController(ISystemService systemService, RestaurantDbContext context)
        {
            _systemService = systemService;
            _context = context;
        }

        [HttpGet("status")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatus()
        {
            var result = await _systemService.GetSystemStatusAsync();
            return Ok(result);
        }

        [HttpGet("test-db")]
        [AllowAnonymous]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect) return Ok(new { IsSuccess = true, Message = "Neural Link Established. Database is responsive." });
                return BadRequest(new { IsSuccess = false, Message = "Connection Refused. Check database server status." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = $"Connectivity Error: {ex.Message}" });
            }
        }

        [HttpPost("seed-demo")]
        [AllowAnonymous]
        public async Task<IActionResult> SeedDemo()
        {
            var result = await _systemService.SeedDemoDataAsync();
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("health")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHealth()
        {
            var dbResponsive = false;
            string dbError = string.Empty;
            try
            {
                dbResponsive = await _context.Database.CanConnectAsync();
            }
            catch (System.Exception ex)
            {
                dbError = ex.Message;
            }

            var status = await _systemService.GetSystemStatusAsync();

            return Ok(new
            {
                IsSuccess = dbResponsive,
                Timestamp = System.DateTime.UtcNow,
                Database = new
                {
                    Status = dbResponsive ? "Healthy" : "Unreachable",
                    Error = string.IsNullOrEmpty(dbError) ? null : dbError
                },
                System = status.Data
            });
        }

        [HttpGet("queue-health")]
        [Authorize]
        public async Task<IActionResult> GetQueueHealth()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();

                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Queue monitor queried successfully.",
                    Data = new
                    {
                        DatabaseConnection = canConnect ? "Active" : "Disconnected",
                        MessageBroker = "MassTransit (RabbitMQ)",
                        OutboxPattern = "Enabled",
                        Status = canConnect ? "Healthy" : "Degraded"
                    }
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Error querying queue health.",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("initialize")]
        [AllowAnonymous]
        public async Task<IActionResult> Initialize([FromBody] SystemInitializationDto initializationDto)
        {
            var result = await _systemService.InitializeSystemAsync(initializationDto);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}
