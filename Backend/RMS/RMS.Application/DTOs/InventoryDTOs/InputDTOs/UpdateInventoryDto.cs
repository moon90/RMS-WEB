using System;

namespace RMS.Application.DTOs.InventoryDTOs.InputDTOs
{
    public class UpdateInventoryDto
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public int InitialStock { get; set; }
        public int MinStockLevel { get; set; }
        public bool Status { get; set; }
    }
}
