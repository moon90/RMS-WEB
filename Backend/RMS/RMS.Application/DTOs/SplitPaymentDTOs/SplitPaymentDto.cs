using System;

namespace RMS.Application.DTOs.SplitPaymentDTOs
{
    public class SplitPaymentDto
    {
        public int SplitPaymentID { get; set; }
        public int SaleID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
