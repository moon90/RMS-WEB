using System;
using RMS.Application.DTOs.ProductDTOs.OutputDTOs;

namespace RMS.Application.DTOs.Orders
{
    public class OrderDetailDto
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal Amount { get; set; }
        public int? PromotionDetailID { get; set; }

        public ProductDto Product { get; set; } // Navigation property for Product
    }
}