using System;

namespace RMS.Domain.Entities
{
    public class SplitPayment : BaseEntity
    {
        public int SplitPaymentID { get; set; }
        public int SaleID { get; set; }
        public Sale Sale { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
