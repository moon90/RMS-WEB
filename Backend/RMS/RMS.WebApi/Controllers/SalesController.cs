
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using System.Threading.Tasks;
using RMS.Application.DTOs;

namespace RMS.Api.Controllers
{
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
        public async Task<IActionResult> Get()
        {
            return Ok(await _saleService.GetSalesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _saleService.GetSaleAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSaleDto saleDto)
        {
            return Ok(await _saleService.CreateSaleAsync(saleDto));
        }
    }
}
