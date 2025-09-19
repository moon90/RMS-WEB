using System;

namespace RMS.Application.DTOs.Orders
{
    public class UpdateOrderDetailDto
    {
        public int OrderDetailID { get; set; } // Required for identifying the detail to update
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; } = 0m;
        public decimal Amount { get; set; }
        public int? PromotionDetailID { get; set; }
    }
}