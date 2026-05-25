using System;
using System.Collections.Generic;
using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public int SaleID { get; set; }
        public DateTime SaleDate { get; set; }
        public int? CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ServiceChargeAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CategoryId { get; set; }
        
        // New Additions
        public string? TokenNumber { get; set; } // Token for Queue System
        public decimal TipAmount { get; set; }   // Track gratuity/tips
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }

        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
        public virtual ICollection<SplitPayment> SplitPayments { get; set; } = new List<SplitPayment>();
    }
}
