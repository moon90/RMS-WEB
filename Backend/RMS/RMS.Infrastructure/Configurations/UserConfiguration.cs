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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.UserName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PasswordHash).IsRequired().HasMaxLength(256);
            builder.Property(e => e.PasswordSalt).IsRequired();
            builder.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Email).HasMaxLength(100);
            builder.Property(e => e.Phone).HasMaxLength(20);

            // Seed initial users
            builder.HasData(

                new User { Id = 1, UserName = "admin", FullName = "System Administrator", Email = "admin@example.com", Phone = "0000000000", PasswordHash = "Yy8eXlyYGaz5Mg6Zvd1nPQ", PasswordSalt = "7xe7O82HP8rVPMyNod2zpg", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow, IsDeleted = false },
                    new User { Id = 2, UserName = "manager", FullName = "Manager User", Email = "manager@example.com", Phone = "0987654321", PasswordHash = "d4QTV4pwUJ-pwL2B2Y4V_w", PasswordSalt = "cZ7UtVxlTYIEb97pOqfoBQ", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow, IsDeleted = false },
                    new User { Id = 3, UserName = "user", FullName = "Standard User", Email = "user@example.com", Phone = "0987654321", PasswordHash = "6Fn94S0iWXBrFbYv5v4Yxg", PasswordSalt = "6Fn94S0iWXBrFbYv5v4Yxg", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow, IsDeleted = false }

            );
        }
    }
}
