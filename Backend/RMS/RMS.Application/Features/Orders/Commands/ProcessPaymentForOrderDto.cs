using RMS.Application.DTOs.SplitPaymentDTOs;
using System.Collections.Generic;

namespace RMS.Application.DTOs.Orders
{
    public class ProcessPaymentForOrderDto
    {
        public int OrderID { get; set; }
        public decimal AmountReceived { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal ChangeAmount { get; set; }
        public string PaymentMethod { get; set; } // e.g., "Cash", "Card", "MobilePay"
        public bool IsSplit { get; set; }
        public List<CreateSplitPaymentDto> SplitPayments { get; set; }
    }
}
