using RMS.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IPaymentGatewayService
    {
        Task<ResponseDto<CheckoutSessionResponseDto>> CreateCheckoutSessionAsync(CreateCheckoutSessionDto dto, CancellationToken cancellationToken = default);
        Task<ResponseDto<PaymentWebhookResultDto>> ProcessStripeWebhookAsync(string jsonPayload, string stripeSignature, CancellationToken cancellationToken = default);
        Task<ResponseDto<RMS.Domain.Entities.PaymentTransaction>> RefundDepositAsync(string transactionReference, decimal refundAmount, string? reason = null, CancellationToken cancellationToken = default);
    }
}
