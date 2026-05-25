using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class Inventory : BaseEntity, IMultiTenant
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public int InitialStock { get; set; }
        public int CurrentStock { get; set; }
        public int MinStockLevel { get; set; }
        public DateTime LastUpdated { get; set; }
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }

        // Navigation property
        public Product? Product { get; set; }
    }
}
