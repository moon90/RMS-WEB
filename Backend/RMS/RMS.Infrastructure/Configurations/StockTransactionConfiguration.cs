using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> builder)
        {
            builder.ToTable("StockTransactions");

            builder.HasKey(st => st.TransactionID);

            builder.Property(st => st.TransactionID)
                .ValueGeneratedOnAdd();

            builder.Property(st => st.TransactionType)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(st => st.Quantity)
                .IsRequired();

            builder.Property(st => st.TransactionDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(st => st.ExpireDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(st => st.Remarks)
                .HasMaxLength(250);

            builder.Property(st => st.TransactionSource)
                .HasMaxLength(50);

            // Configure common BaseEntity properties
            builder.Property(st => st.Status)
                .HasDefaultValue(true);

            builder.Property(st => st.CreatedBy)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("system");

            builder.Property(st => st.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(st => st.IsDeleted)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(st => st.Product)
                .WithMany()
                .HasForeignKey(st => st.ProductID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(st => st.Supplier)
                .WithMany()
                .HasForeignKey(st => st.SupplierID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            builder.HasData(
                new StockTransaction
                {
                    TransactionID = 1,
                    ProductID = 1, // Assuming Product with ID 1 exists
                    SupplierID = null, // No supplier for this transaction
                    TransactionType = "IN",
                    Quantity = 10,
                    TransactionDate = DateTime.UtcNow,
                    ExpireDate = DateTime.UtcNow.AddMonths(6),
                    Remarks = "Initial stock received",
                    SaleID = null,
                    PurchaseID = null,
                    TransactionSource = "Purchase",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new StockTransaction
                {
                    TransactionID = 2,
                    ProductID = 2, // Assuming Product with ID 2 exists
                    SupplierID = 1, // Assuming Supplier with ID 1 exists
                    TransactionType = "OUT",
                    Quantity = 5,
                    TransactionDate = DateTime.UtcNow,
                    ExpireDate = null,
                    Remarks = "Sold to customer",
                    SaleID = 1, // Assuming Sale with ID 1 exists
                    PurchaseID = null,
                    TransactionSource = "Sale",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}
