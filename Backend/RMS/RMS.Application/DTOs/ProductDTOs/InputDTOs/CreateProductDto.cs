
using System;

namespace RMS.Application.DTOs.ProductDTOs.InputDTOs
{
    public class CreateProductDto
    {
        public required string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal? CostPrice { get; set; }
        public string? ProductBarcode { get; set; }
        public string? ProductImage { get; set; }
        public string? ThumbnailImage { get; set; }
        public int? CategoryID { get; set; }
        public int? SupplierID { get; set; }
        public int? ManufacturerID { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool Status { get; set; }
    }
}
