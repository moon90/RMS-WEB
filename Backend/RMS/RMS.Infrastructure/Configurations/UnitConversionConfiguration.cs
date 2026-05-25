using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class UnitConversionConfiguration : IEntityTypeConfiguration<UnitConversion>
    {
        public void Configure(EntityTypeBuilder<UnitConversion> builder)
        {
            builder.ToTable("UnitConversions");

            builder.HasKey(uc => uc.UnitConversionID);

            builder.Property(uc => uc.ConversionFactor)
                .HasColumnType("DECIMAL(18,4)");

            builder.HasOne(uc => uc.FromUnit)
                .WithMany()
                .HasForeignKey(uc => uc.FromUnitID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(uc => uc.ToUnit)
                .WithMany()
                .HasForeignKey(uc => uc.ToUnitID)
                .OnDelete(DeleteBehavior.Restrict);

            // Sample Data
            builder.HasData(
                new UnitConversion { UnitConversionID = 1, FromUnitID = 1, ToUnitID = 4, ConversionFactor = 1000m }, // kg to g
                new UnitConversion { UnitConversionID = 2, FromUnitID = 2, ToUnitID = 5, ConversionFactor = 1000m }  // L to ml
            );
        }
    }
}
