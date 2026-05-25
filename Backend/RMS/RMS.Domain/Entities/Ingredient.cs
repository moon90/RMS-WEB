using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class Ingredient : BaseEntity, IMultiTenant
    {
        public int IngredientID { get; set; }
        public required string Name { get; set; }
        public decimal QuantityAvailable { get; set; }
        public decimal CostPrice { get; set; } // Per unit cost
        public int UnitID { get; set; }
        public decimal ReorderLevel { get; set; }
        public decimal ReorderQuantity { get; set; }
        public int? SupplierID { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? Remarks { get; set; }
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }

        // Navigation properties
        public Unit? Unit { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
