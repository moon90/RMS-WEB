using System;

namespace RMS.Application.DTOs.Promotions
{
    public class CreatePromotionDto
    {
        public string CouponCode { get; set; }

        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }

        public string? Description { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public bool IsActive { get; set; } = true;
    }
}