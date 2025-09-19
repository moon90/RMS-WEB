
using System;
using System.Collections.Generic;

namespace RMS.Domain.Entities
{
    public class Purchase : BaseEntity
    {
        public int PurchaseID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int SupplierID { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
    }
}
