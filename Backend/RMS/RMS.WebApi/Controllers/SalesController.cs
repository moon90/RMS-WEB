using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
using System.Threading.Tasks;
using RMS.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        [Authorize(Policy = "SALE_VIEW")]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortDirection = null)
        {
            var result = await _saleService.GetAllSalesAsync(pageNumber, pageSize, searchQuery, sortColumn, sortDirection);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "SALE_VIEW")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _saleService.GetSaleAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "SALE_CREATE")]
        public async Task<IActionResult> Post([FromBody] CreateSaleDto saleDto)
        {
            var result = await _saleService.CreateSaleAsync(saleDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "SALE_UPDATE")]
        public async Task<IActionResult> Put(int id, [FromBody] CreateSaleDto saleDto)
        {
            var result = await _saleService.UpdateSaleAsync(id, saleDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "SALE_DELETE")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _saleService.DeleteSaleAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
