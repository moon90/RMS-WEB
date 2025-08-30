
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
                new Supplier { Id = 1, SupplierName = "Supplier A", ContactPerson = "Person A", Phone = "1234567890", Email = "supplier.a@example.com", Address = "Address A", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Supplier { Id = 2, SupplierName = "Supplier B", ContactPerson = "Person B", Phone = "0987654321", Email = "supplier.b@example.com", Address = "Address B", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Supplier { Id = 3, SupplierName = "Supplier C", ContactPerson = "Person C", Phone = "1122334455", Email = "supplier.c@example.com", Address = "Address C", Status = false, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
            );
        }
    }
}
