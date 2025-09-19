using System;

namespace RMS.Application.DTOs.StockTransactionDTOs.InputDTOs
{
    public class UpdateStockTransactionDto
    {
        public int TransactionID { get; set; }
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
        public bool Status { get; set; }
    }
}
