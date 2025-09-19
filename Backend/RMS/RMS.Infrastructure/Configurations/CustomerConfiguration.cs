using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.CustomerID);

            builder.Property(c => c.CustomerID)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CustomerPhone)
                .HasMaxLength(20);

            builder.Property(c => c.CustomerEmail)
                .HasMaxLength(100);

            builder.Property(c => c.Address)
                .HasMaxLength(250);

            builder.Property(c => c.DriverName)
                .HasMaxLength(100);

            // Configure common BaseEntity properties
            builder.Property(c => c.Status)
                .HasDefaultValue(true);

            builder.Property(c => c.CreatedBy)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("system");

            builder.Property(c => c.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            // Seed Data
            builder.HasData(
                new Customer
                {
                    CustomerID = 1,
                    CustomerName = "John Doe",
                    CustomerPhone = "123-456-7890",
                    CustomerEmail = "john.doe@example.com",
                    Address = "123 Main St, Anytown",
                    DriverName = "John Doe",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Customer
                {
                    CustomerID = 2,
                    CustomerName = "Jane Smith",
                    CustomerPhone = "098-765-4321",
                    CustomerEmail = "jane.smith@example.com",
                    Address = "456 Oak Ave, Somewhere",
                    DriverName = "Jane Smith",
                    Status = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}
