namespace RMS.Application.DTOs.SplitPaymentDTOs
{
    public class CreateSplitPaymentDto
    {
        public int SaleID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
