using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.ProductPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.CostPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.ProductBarcode)
                .HasMaxLength(100);

            builder.Property(p => p.ProductImage)
                .HasColumnType("varbinary(max)");

            builder.Property(p => p.ThumbnailImage)
                .HasColumnType("varbinary(max)");

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryID)
                .IsRequired(false);

            builder.HasOne(p => p.Supplier)
                .WithMany()
                .HasForeignKey(p => p.SupplierID)
                .IsRequired(false);

            builder.HasOne(p => p.Manufacturer)
                .WithMany()
                .HasForeignKey(p => p.ManufacturerID)
                .IsRequired(false);

            builder.HasOne(p => p.Branch)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BranchID)
                .IsRequired(false);

            builder.Property(p => p.ExpireDate)
                .HasColumnType("date");

            // Seed
            builder.HasData(
                new Product { Id = 1, ProductName = "Signature Wagyu Burger", ProductPrice = 28.50m, CostPrice = 12.00m, ProductBarcode = "PRD-WGY-01", CategoryID = 1, SupplierID = 1, ManufacturerID = 1, ExpireDate = DateTime.UtcNow.AddYears(1), Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Product { Id = 2, ProductName = "Atlantic Lobster Pasta", ProductPrice = 42.00m, CostPrice = 18.50m, ProductBarcode = "PRD-LOB-02", CategoryID = 2, SupplierID = 2, ManufacturerID = 2, ExpireDate = DateTime.UtcNow.AddYears(2), Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Product { Id = 3, ProductName = "Wild Mushroom Truffle Risotto", ProductPrice = 34.00m, CostPrice = 14.20m, ProductBarcode = "PRD-RIS-03", CategoryID = 1, SupplierID = 1, ManufacturerID = 3, ExpireDate = DateTime.UtcNow.AddYears(3), Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Product { Id = 4, ProductName = "Aged Ribeye Steak (400g)", ProductPrice = 58.00m, CostPrice = 24.00m, ProductBarcode = "PRD-RB-04", CategoryID = 1, SupplierID = 1, ManufacturerID = 1, ExpireDate = DateTime.UtcNow.AddYears(1), Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
            );
        }
    }
}