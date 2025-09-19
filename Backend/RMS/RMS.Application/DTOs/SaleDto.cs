
using System;
using System.Collections.Generic;

namespace RMS.Application.DTOs
{
    public class SaleDto
    {
        public int SaleID { get; set; }
        public DateTime SaleDate { get; set; }
        public int? CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SaleDetailDto> SaleDetails { get; set; }
    }

    public class SaleDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
