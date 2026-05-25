
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.SupplierName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.ContactPerson)
                .HasMaxLength(100);

            builder.Property(s => s.Phone)
                .HasMaxLength(20);

            builder.Property(s => s.Email)
                .HasMaxLength(100);

            builder.Property(s => s.Address)
                .HasMaxLength(250);

            // Seed
            builder.HasData(
                new Supplier { Id = 1, SupplierName = "Global Wagyu Select", ContactPerson = "James Tanaka", Phone = "+81-3-1234-5678", Email = "orders@wagyu-select.jp", Address = "4-12-1 Ginza, Chuo-ku, Tokyo", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Supplier { Id = 2, SupplierName = "Mediterranean Fine Oils", ContactPerson = "Sofia Rossi", Phone = "+39-06-9876-5432", Email = "exports@rossi-oils.it", Address = "Via dei Condotti, 12, Rome", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Supplier { Id = 3, SupplierName = "Express Coffee Roasters", ContactPerson = "Mark Brew", Phone = "+1-212-555-0199", Email = "wholesale@express-roasts.com", Address = "789 Roaster Row, Brooklyn, NY", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
            );
        }
    }
}
