using System;
using System.Collections.Generic;
using RMS.Core.Enum;
using RMS.Application.Interfaces;

namespace RMS.Application.DTOs
{
    public class StockTransferDto
    {
        public int StockTransferID { get; set; }
        public DateTime TransferDate { get; set; }
        public int FromBranchID { get; set; }
        public string FromBranchName { get; set; }
        public int ToBranchID { get; set; }
        public string ToBranchName { get; set; }
        public string Status { get; set; }
        public string? Remarks { get; set; }
        public string? TransferNumber { get; set; }
        public List<StockTransferDetailDto> Details { get; set; }
    }

    public class StockTransferDetailDto
    {
        public int IngredientID { get; set; }
        public string IngredientName { get; set; }
        public decimal Quantity { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
    }

    public class CreateStockTransferDto
    {
        public int ToBranchID { get; set; }
        public string? Remarks { get; set; }
        public List<CreateStockTransferDetailDto> Details { get; set; }
    }

    public class CreateStockTransferDetailDto
    {
        public int IngredientID { get; set; }
        public decimal Quantity { get; set; }
    }
}
