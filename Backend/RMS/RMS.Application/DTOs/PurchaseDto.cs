
using System;
using System.Collections.Generic;

namespace RMS.Application.DTOs
{
    public class PurchaseDto
    {
        public int PurchaseID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int SupplierID { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<PurchaseDetailDto> PurchaseDetails { get; set; }
    }

    public class PurchaseDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
