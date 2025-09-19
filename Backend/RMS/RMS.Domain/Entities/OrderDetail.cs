using System;

namespace RMS.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; } // Foreign Key to Order
        public int ProductID { get; set; } // Foreign Key to Product
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal Amount { get; set; }
        public int? PromotionDetailID { get; set; } // If promotions can apply per detail line

        // Navigation properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}