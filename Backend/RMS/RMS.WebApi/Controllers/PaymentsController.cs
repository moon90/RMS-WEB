using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentGatewayService _paymentService;

        public PaymentsController(IPaymentGatewayService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("checkout-session")]
        [Authorize]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionDto dto, CancellationToken cancellationToken)
        {
            var response = await _paymentService.CreateCheckoutSessionAsync(dto, cancellationToken);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> ProcessStripeWebhook(CancellationToken cancellationToken)
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var jsonPayload = await reader.ReadToEndAsync(cancellationToken);
            var stripeSignature = Request.Headers["Stripe-Signature"].ToString();

            var response = await _paymentService.ProcessStripeWebhookAsync(jsonPayload, stripeSignature, cancellationToken);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
