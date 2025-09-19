using System;

namespace RMS.Application.DTOs.RealtimeUpdates
{
    public class OrderUpdateDto
    {
        public int OrderId { get; set; }
        public string TableName { get; set; }
        public string OrderStatus { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string OrderType { get; set; } // e.g., "DineIn", "TakeOut", "Delivery"
    }
}
