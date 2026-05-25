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
