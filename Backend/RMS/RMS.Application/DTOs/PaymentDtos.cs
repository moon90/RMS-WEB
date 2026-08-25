using System;

namespace RMS.Application.DTOs
{
    public class CreateCheckoutSessionDto
    {
        public Guid ReservationId { get; set; }
    }

    public class CheckoutSessionResponseDto
    {
        public string ClientSecret { get; set; } = string.Empty;
        public string PublishableKey { get; set; } = string.Empty;
        public string TransactionReference { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
    }

    public class PaymentWebhookResultDto
    {
        public bool Processed { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? ReservationId { get; set; }
        public string? NewReservationStatus { get; set; }
    }
}
