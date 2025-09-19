using System;

namespace RMS.Application.DTOs.IngredientDTOs.InputDTOs
{
    public class UpdateIngredientDto
    {
        public int IngredientID { get; set; }
        public required string Name { get; set; }
        public decimal QuantityAvailable { get; set; }
        public int UnitID { get; set; }
        public decimal ReorderLevel { get; set; }
        public decimal ReorderQuantity { get; set; }
        public int? SupplierID { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? Remarks { get; set; }
        public bool Status { get; set; }
    }
}
