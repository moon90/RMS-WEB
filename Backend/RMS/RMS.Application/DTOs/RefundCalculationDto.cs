using System;

namespace RMS.Application.DTOs
{
    public class RefundCalculationDto
    {
        public Guid ReservationId { get; set; }
        public DateTime ReservationStartTime { get; set; }
        public double HoursNotice { get; set; }
        public decimal DepositAmount { get; set; }
        public int RefundPercentage { get; set; }
        public decimal RefundAmount { get; set; }
        public decimal ForfeitAmount { get; set; }
        public string PolicyTierDescription { get; set; } = string.Empty;
    }
}
