
using System;

namespace RMS.Application.DTOs.ProductDTOs.OutputDTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal? CostPrice { get; set; }
        public string? ProductBarcode { get; set; }
        public byte[]? ProductImage { get; set; }
        public byte[]? ThumbnailImage { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? CategoryID { get; set; }
        public int? SupplierID { get; set; }
        public int? ManufacturerID { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool Status { get; set; }
        public string? CategoryName { get; set; }
        public string? SupplierName { get; set; }
        public string? ManufacturerName { get; set; }
    }
}
