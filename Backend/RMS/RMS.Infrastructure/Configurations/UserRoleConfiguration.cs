using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User)
                   .WithMany(u => u.UserRoles)
                   .HasForeignKey(e => e.UserID);

            builder.HasOne(e => e.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(e => e.RoleID);

            builder.Property(rm => rm.AssignedBy)
                    .IsRequired(false);

            // Seed initial user roles
            builder.HasData(
                    new UserRole { Id = 1, UserID = 1, RoleID = 1, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Admin user -> Admin role
                    new UserRole { Id = 2, UserID = 2, RoleID = 2, AssignedBy = "System", AssignedAt = DateTime.UtcNow }, // Manager user -> Manager role
                    new UserRole { Id = 3, UserID = 3, RoleID = 3, AssignedBy = "System", AssignedAt = DateTime.UtcNow }  // Standard user -> User role
                );
        }
    }
}
