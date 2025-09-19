using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs.SplitPaymentDTOs;
using RMS.Application.Interfaces;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SplitPaymentController : ControllerBase
    {
        private readonly ISplitPaymentService _splitPaymentService;

        public SplitPaymentController(ISplitPaymentService splitPaymentService)
        {
            _splitPaymentService = splitPaymentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSplitPayment(CreateSplitPaymentDto dto)
        {
            var splitPayment = await _splitPaymentService.CreateSplitPaymentAsync(dto);
            return Ok(splitPayment);
        }
    }
}
