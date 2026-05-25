using System.Collections.Generic;
using RMS.Domain.Models.BaseModels;

namespace RMS.Application.DTOs.InventoryDTOs.OutputDTOs
{
    public class LowStockResultDto
    {
        public PagedResult<InventoryDto> PagedData { get; set; }
        public int CriticalItemsCount { get; set; } // items with 0 stock
        public int WarningItemsCount { get; set; } // items below min level but > 0
        public decimal TotalRestockInvestment { get; set; } // Estimated cost to bring all to initial level
    }
}
