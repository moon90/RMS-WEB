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
    public class StockTransfersController : ControllerBase
    {
        private readonly IStockTransferService _transferService;

        public StockTransfersController(IStockTransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchQuery = null,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = null)
        {
            var result = await _transferService.GetAllTransfersAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _transferService.GetTransferByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockTransferDto transferDto)
        {
            var result = await _transferService.CreateTransferAsync(transferDto);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _transferService.UpdateTransferStatusAsync(id, status);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
    }
}
