namespace RMS.Application.DTOs.StockTransactionDTOs.InputDTOs
{
    public class ReceiveStockDto
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int? SupplierID { get; set; }
        public string? Remarks { get; set; }
    }
}