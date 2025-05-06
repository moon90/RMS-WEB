using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.PermissionName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PermissionKey).HasMaxLength(100);

            // Seed
            builder.HasData(
                new Permission { Id = 1, PermissionName = "ViewDashboard", PermissionKey = "DASHBOARD", Status = true },
                new Permission { Id = 2, PermissionName = "ManageUsers", PermissionKey = "MANAGE_USERS", Status = true }
            );
        }
    }
}
