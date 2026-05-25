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
    public class InventoryAuditsController : ControllerBase
    {
        private readonly IInventoryAuditService _auditService;

        public InventoryAuditsController(IInventoryAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _auditService.GetAllAuditsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _auditService.GetAuditByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInventoryAuditDto auditDto)
        {
            var result = await _auditService.CreateAuditAsync(auditDto);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
    }
}
