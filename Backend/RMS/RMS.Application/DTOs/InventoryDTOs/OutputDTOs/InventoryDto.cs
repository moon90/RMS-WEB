using System;

namespace RMS.Application.DTOs.InventoryDTOs.OutputDTOs
{
    public class InventoryDto
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int InitialStock { get; set; }
        public int MinStockLevel { get; set; }
        public int CurrentStock { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Status { get; set; }
    }
}
