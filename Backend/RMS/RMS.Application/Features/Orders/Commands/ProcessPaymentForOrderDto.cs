using RMS.Application.DTOs.SplitPaymentDTOs;
using System.Collections.Generic;
using RMS.Application.Interfaces;

namespace RMS.Application.DTOs.Orders
{
    public class ProcessPaymentForOrderDto
    {
        public int OrderID { get; set; }
        public decimal AmountReceived { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal ChangeAmount { get; set; }
        public string PaymentMethod { get; set; } // e.g., "Cash", "Card", "MobilePay"
        public decimal DiscountAmount { get; set; }
        public decimal TipAmount { get; set; }
        public int? PromotionID { get; set; }
        public bool IsSplit { get; set; }
        public List<CreateSplitPaymentDto>? SplitPayments { get; set; }
    }
}
