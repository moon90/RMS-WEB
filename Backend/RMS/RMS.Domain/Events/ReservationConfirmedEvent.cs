using System;

namespace RMS.Domain.Events
{
    public class ReservationConfirmedEvent
    {
        public Guid ReservationId { get; set; }
        public int DiningTableId { get; set; }
        public int CustomerId { get; set; }
        public decimal DepositAmount { get; set; }
        public string TransactionReference { get; set; } = string.Empty;
        public DateTime ConfirmedAt { get; set; } = DateTime.UtcNow;
    }
}
