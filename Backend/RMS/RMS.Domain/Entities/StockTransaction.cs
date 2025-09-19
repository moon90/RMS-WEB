
namespace RMS.Domain.Entities
{
    public class StockTransaction : BaseEntity
    {
        public int TransactionID { get; set; }
        public int? ProductID { get; set; }
        public int? SupplierID { get; set; }
        public required string TransactionType { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? Remarks { get; set; }
        public int? SaleID { get; set; }
        public int? PurchaseID { get; set; }
        public string? TransactionSource { get; set; }
        public string? AdjustmentType { get; set; } // e.g., "Addition", "Subtraction", null for non-adjustments
        public string? Reason { get; set; } // Reason for adjustment
        public int? IngredientID { get; set; } // New field for tracking consumed ingredients

        // Navigation properties
        public Product? Product { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
