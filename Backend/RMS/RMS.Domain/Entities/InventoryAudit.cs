using System;
using System.Collections.Generic;
using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class InventoryAudit : BaseEntity, IMultiTenant
    {
        public int InventoryAuditID { get; set; }
        public DateTime AuditDate { get; set; }
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }
        public string? AuditorName { get; set; }
        public string? Remarks { get; set; }

        public virtual ICollection<InventoryAuditDetail> Details { get; set; } = new List<InventoryAuditDetail>();
    }

    public class InventoryAuditDetail : BaseEntity
    {
        public int InventoryAuditDetailID { get; set; }
        public int InventoryAuditID { get; set; }
        public int IngredientID { get; set; }
        public decimal TheoreticalStock { get; set; } // What system says
        public decimal PhysicalStock { get; set; }    // What was counted
        public decimal Variance => PhysicalStock - TheoreticalStock;
        public decimal VarianceValue { get; set; } // Financial value of variance

        public virtual InventoryAudit Audit { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}
