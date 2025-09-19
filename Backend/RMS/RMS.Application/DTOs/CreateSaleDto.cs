
using System;
using System.Collections.Generic;

namespace RMS.Application.DTOs
{
    public class CreateSaleDto
    {
        public DateTime SaleDate { get; set; }
        public int? CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public int CategoryId { get; set; }
        public List<SaleDetailDto> SaleDetails { get; set; }
    }
}
