
namespace RMS.Domain.Entities
{
    public class Inventory : BaseEntity
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public int InitialStock { get; set; }
        public int CurrentStock { get; set; }
        public int MinStockLevel { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation property
        public Product? Product { get; set; }
    }
}
