using System;
using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class TableReservation : BaseEntity, IMultiTenant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int DiningTableId { get; set; }
        public virtual DiningTable? DiningTable { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public DateTime ReservationStartTime { get; set; }
        public DateTime ReservationEndTime { get; set; }
        public DateTime HoldExpiresAt { get; set; }
        public string ReservationStatus { get; set; } = "PendingPayment";
        public decimal DepositAmount { get; set; }
        public decimal RefundAmount { get; set; } = 0m;
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}
