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
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpGet]
        [Authorize(Policy = "ROLE_VIEW")] // Only admins/HR can view payroll
        public async Task<IActionResult> GetAll()
        {
            var result = await _payrollService.GetAllPayrollsAsync();
            return Ok(result);
        }

        [HttpPost("run-ai")]
        [Authorize(Policy = "ROLE_CREATE")]
        public async Task<IActionResult> RunAi([FromBody] CreatePayrollDto runDto)
        {
            var result = await _payrollService.RunPayrollAiAsync(runDto);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}
