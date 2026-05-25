using System;
using System.Collections.Generic;
using RMS.Core.Enum;
using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class Purchase : BaseEntity, IMultiTenant
    {
        public int PurchaseID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int SupplierID { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CategoryId { get; set; }
        public PurchaseStatus PurchaseStatus { get; set; } = PurchaseStatus.Draft;
        public string? Remarks { get; set; }
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }

        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
    }
}
