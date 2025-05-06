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

                new User
                {
                    Id = 1,
                    UserName = "admin",
                    PasswordHash = "c4vD3op8WBxFFjk_XPZoHA", // Replace with actual hashed password
                    PasswordSalt = "qtixrauL4wM-8gdAhr6rAA", // Replace with actual password salt
                    FullName = "Admin User",
                    Email = "admin@example.com",
                    Phone = "1234567890",
                    RefreshToken = null,
                    RefreshTokenExpiry = null,
                    Status = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new User
                {
                    Id = 2,
                    UserName = "manager",
                    PasswordHash = "d4QTV4pwUJ-pwL2B2Y4V_w", // Replace with actual hashed password
                    PasswordSalt = "cZ7UtVxlTYIEb97pOqfoBQ", // Replace with actual password salt
                    FullName = "Manager User",
                    Email = "manager@example.com",
                    Phone = "0987654321",
                    RefreshToken = null,
                    RefreshTokenExpiry = null,
                    Status = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new User
                {
                    Id = 3,
                    UserName = "user",
                    PasswordHash = "6Fn94S0iWXBrFbYv5v4Yxg", // Replace with actual hashed password
                    PasswordSalt = "6Fn94S0iWXBrFbYv5v4Yxg", // Replace with actual password salt
                    FullName = "User",
                    Email = "user@example.com",
                    Phone = "0987654321",
                    RefreshToken = null,
                    RefreshTokenExpiry = null,
                    Status = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                }

            );
        }
    }
}
