using System;

namespace RMS.Application.DTOs
{
    public class TableReservationDto
    {
        public Guid Id { get; set; }
        public int DiningTableId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReservationStartTime { get; set; }
        public DateTime ReservationEndTime { get; set; }
        public DateTime HoldExpiresAt { get; set; }
        public string ReservationStatus { get; set; } = string.Empty;
        public decimal DepositAmount { get; set; }
    }

    public class CreateReservationHoldDto
    {
        public int DiningTableId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReservationStartTime { get; set; }
        public DateTime ReservationEndTime { get; set; }
        public decimal DepositAmount { get; set; }
    }
}
