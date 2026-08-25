using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class TableReservationConfiguration : IEntityTypeConfiguration<TableReservation>
    {
        public void Configure(EntityTypeBuilder<TableReservation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.DepositAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.RefundAmount)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            builder.Property(r => r.CancellationReason)
                .HasMaxLength(500);

            builder.Property(r => r.ReservationStatus)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(r => r.RowVersion)
                .IsRowVersion();

            builder.HasOne(r => r.DiningTable)
                .WithMany()
                .HasForeignKey(r => r.DiningTableId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => new { r.DiningTableId, r.ReservationStartTime, r.ReservationEndTime });
            builder.HasIndex(r => new { r.ReservationStatus, r.HoldExpiresAt });
        }
    }
}
