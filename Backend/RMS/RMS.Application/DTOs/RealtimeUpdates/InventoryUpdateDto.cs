using System;

namespace RMS.Application.DTOs.RealtimeUpdates
{
    public class InventoryUpdateDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal OldQuantity { get; set; }
        public decimal NewQuantity { get; set; }
        public string ChangeType { get; set; } // e.g., "Consumed", "Received", "Adjusted"
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
