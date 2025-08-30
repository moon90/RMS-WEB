
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.ToTable("Manufacturers");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.ManufacturerName)
                .HasMaxLength(100);

            // Seed
            builder.HasData(
                new Manufacturer { Id = 1, ManufacturerName = "Manufacturer X", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Manufacturer { Id = 2, ManufacturerName = "Manufacturer Y", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Manufacturer { Id = 3, ManufacturerName = "Manufacturer Z", Status = false, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
            );
        }
    }
}
