using System;
using System.Collections.Generic;
using RMS.Application.Interfaces;

namespace RMS.Application.DTOs
{
    public class InventoryAuditDto
    {
        public int InventoryAuditID { get; set; }
        public DateTime AuditDate { get; set; }
        public string? AuditorName { get; set; }
        public string? Remarks { get; set; }
        public decimal TotalVarianceValue { get; set; }
        public List<InventoryAuditDetailDto> Details { get; set; }
    }

    public class InventoryAuditDetailDto
    {
        public int IngredientID { get; set; }
        public string IngredientName { get; set; }
        public decimal TheoreticalStock { get; set; }
        public decimal PhysicalStock { get; set; }
        public decimal Variance { get; set; }
        public decimal VarianceValue { get; set; }
    }

    public class CreateInventoryAuditDto
    {
        public string? AuditorName { get; set; }
        public string? Remarks { get; set; }
        public List<CreateInventoryAuditDetailDto> Details { get; set; }
    }

    public class CreateInventoryAuditDetailDto
    {
        public int IngredientID { get; set; }
        public decimal PhysicalStock { get; set; }
    }
}
