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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.RoleName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(250);

            // Seed
            builder.HasData(
                new Role { Id = 1, RoleName = "Admin", Description = "Administrator role", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Role { Id = 2, RoleName = "Manager", Description = "Standard manager role", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Role { Id = 3, RoleName = "User", Description = "Standard user role", CreatedBy = "system", CreatedDate = DateTime.UtcNow }
            );
        }
    }
}
