namespace RMS.Application.DTOs.StockTransactionDTOs.InputDTOs
{
    public class AdjustStockDto
    {
        public int ProductID { get; set; }
        public int QuantityChange { get; set; } // Positive for increase, negative for decrease
        public string AdjustmentType { get; set; } // e.g., "Correction", "Damage", "Return"
        public string? Remarks { get; set; }
    }
}