using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.ToTable("Alerts");

            builder.HasKey(a => a.AlertId);

            builder.Property(a => a.AlertId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Message)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(a => a.Type)
                .IsRequired();

            builder.Property(a => a.IsAcknowledged)
                .IsRequired();

            builder.Property(a => a.AlertDate)
                .IsRequired();
        }
    }
}
