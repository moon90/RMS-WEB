using System;

namespace RMS.Application.DTOs.StockTransactionDTOs.InputDTOs
{
    public class CreateStockTransactionDto
    {
        public int? ProductID { get; set; }
        public int? SupplierID { get; set; }
        public required string TransactionType { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? Remarks { get; set; }
        public int? SaleID { get; set; }
        public int? PurchaseID { get; set; }
        public string? TransactionSource { get; set; }
        public string? AdjustmentType { get; set; }
        public string? Reason { get; set; }
        public int? IngredientID { get; set; }
    }
}
