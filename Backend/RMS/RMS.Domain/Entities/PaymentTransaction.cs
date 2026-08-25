using System;
using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class PaymentTransaction : BaseEntity, IMultiTenant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TableReservationId { get; set; }
        public virtual TableReservation? TableReservation { get; set; }
        public string PaymentGateway { get; set; } = "Stripe";
        public string TransactionReference { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Status { get; set; } = "Processing";
        public decimal? RefundAmount { get; set; }
        public DateTime? RefundedAt { get; set; }
        public string? StripeRefundId { get; set; }
        public string? WebhookEventId { get; set; }
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}
