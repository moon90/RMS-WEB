using System;

namespace RMS.Domain.Events
{
    public class ReservationCancelledEvent
    {
        public Guid ReservationId { get; set; }
        public int DiningTableId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal RefundAmount { get; set; }
        public string? TransactionReference { get; set; }
        public string? Reason { get; set; }
        public DateTime CancelledAt { get; set; } = DateTime.UtcNow;
    }
}
