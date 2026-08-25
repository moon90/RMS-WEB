using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PaymentGateway)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.TransactionReference)
                .HasMaxLength(100)
                .IsRequired();

            // AC-4: Unique index for Webhook/Transaction idempotency
            builder.HasIndex(p => p.TransactionReference)
                .IsUnique();

            builder.Property(p => p.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.Currency)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(p => p.Status)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.WebhookEventId)
                .HasMaxLength(100);

            builder.HasOne(p => p.TableReservation)
                .WithMany()
                .HasForeignKey(p => p.TableReservationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Branch)
                .WithMany()
                .HasForeignKey(p => p.BranchID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
