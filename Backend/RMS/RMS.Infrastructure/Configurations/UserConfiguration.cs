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

                new User { Id = 1, UserName = "admin", FullName = "System Administrator", Email = "admin@rms-global.com", Phone = "555-0100", BranchID = 1, PasswordHash = "Yy8eXlyYGaz5Mg6Zvd1nPfIYqYNZZKXGB2sxltdw0mA", PasswordSalt = "7xe7O82HP8rVPMyNod2zpg", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow, IsDeleted = false },
                new User { Id = 2, UserName = "manager", FullName = "Downtown Manager", Email = "manager@rms-global.com", Phone = "555-0200", BranchID = 2, PasswordHash = "d4QTV4pwUJ-pwL2B2Y4V__cxvF0Wgj4uLh8DSEyJJbc", PasswordSalt = "cZ7UtVxlTYIEb97pOqfoBQ", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow, IsDeleted = false },
                new User { Id = 3, UserName = "user", FullName = "Standard Staff", Email = "staff@rms-global.com", Phone = "555-0300", BranchID = 1, PasswordHash = "KnaR7zPI94wxj7LIa9cwOB6Nb84RkgY1CHuGVESSWcg", PasswordSalt = "6Fn94S0iWXBrFbYv5v4Yxg", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow, IsDeleted = false }

            );
        }
    }
}
