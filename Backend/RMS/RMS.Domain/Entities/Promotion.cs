using System;
using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class Promotion : BaseEntity, IMultiTenant
    {
        public int PromotionID { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string? Description { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}