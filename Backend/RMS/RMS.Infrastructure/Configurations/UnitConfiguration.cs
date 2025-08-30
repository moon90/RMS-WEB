using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Units");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.ShortCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasData(
                new Unit { Id = 1, Name = "Pieces", ShortCode = "pcs", Status = true },
                new Unit { Id = 2, Name = "Kilograms", ShortCode = "kg", Status = true },
                new Unit { Id = 3, Name = "Liters", ShortCode = "l", Status = true },
                new Unit { Id = 4, Name = "Grams", ShortCode = "g", Status = true },
                new Unit { Id = 5, Name = "Milliliters", ShortCode = "ml", Status = true }
            );
        }
    }
}
