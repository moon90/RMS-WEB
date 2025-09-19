
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using System.Threading.Tasks;

namespace RMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchasesController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _purchaseService.GetAllPurchasesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _purchaseService.GetPurchaseByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePurchaseDto purchaseDto)
        {
            return Ok(await _purchaseService.CreatePurchaseAsync(purchaseDto));
        }
    }
}
