using System;

namespace RMS.Application.DTOs.StockTransactionDTOs.OutputDTOs
{
    public class StockTransactionDto
    {
        public int TransactionID { get; set; }
        public int? ProductID { get; set; }
        public string? ProductName { get; set; }
        public int? SupplierID { get; set; }
        public string? SupplierName { get; set; }
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
        public bool Status { get; set; }
    }
}
