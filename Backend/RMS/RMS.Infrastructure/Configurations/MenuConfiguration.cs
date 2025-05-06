using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace RMS.Infrastructure.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.MenuName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.MenuPath).IsRequired().HasMaxLength(200);
            builder.Property(e => e.MenuIcon).HasMaxLength(100);

            builder.HasOne(m => m.ParentMenu)
                   .WithMany(m => m.ChildMenus)
                   .HasForeignKey(m => m.ParentID)
                   .OnDelete(DeleteBehavior.Restrict);

            // Seed initial menus
            builder.HasData(
                new Menu
                {
                    Id = 1,
                    MenuName = "Dashboard",
                    MenuPath = "/dashboard",
                    MenuIcon = "dashboard",
                    DisplayOrder = 1
                },
                new Menu
                {
                    Id = 2,
                    MenuName = "Users",
                    MenuPath = "/users",
                    MenuIcon = "users",
                    DisplayOrder = 2
                }
            );
        }
    }
}
