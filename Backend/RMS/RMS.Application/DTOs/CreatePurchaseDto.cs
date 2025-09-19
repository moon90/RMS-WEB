
using System;
using System.Collections.Generic;

namespace RMS.Application.DTOs
{
    public class CreatePurchaseDto
    {
        public DateTime PurchaseDate { get; set; }
        public int SupplierId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public int CategoryId { get; set; }
        public List<PurchaseDetailDto> PurchaseDetails { get; set; }
    }
}
