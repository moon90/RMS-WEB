
using RMS.Domain.Entities;
using System;

namespace RMS.Domain.Entities
{
    public class Product : BaseEntity
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

        // Navigation properties
        public Category? Category { get; set; }
        public Supplier? Supplier { get; set; }
        public Manufacturer? Manufacturer { get; set; }
    }
}
