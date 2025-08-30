using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.CategoryID);

            builder.Property(c => c.CategoryID)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            // Configure BaseEntity properties if not handled by a convention or base configuration
            builder.Property(c => c.Status).HasDefaultValue(true);
            builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(100).HasDefaultValue("system");
            builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.ModifiedBy).HasMaxLength(100);
            builder.Property(c => c.ModifiedDate).IsRequired(false); // Allow nulls for UpdatedDate
            builder.Property(c => c.IsDeleted).HasDefaultValue(false);

            // Seed Categories
            builder.HasData(
                new Category { CategoryID = 1, CategoryName = "Appetizers", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow, IsDeleted = false },
                new Category { CategoryID = 2, CategoryName = "Main Courses", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow, IsDeleted = false },
                new Category { CategoryID = 3, CategoryName = "Desserts", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow, IsDeleted = false },
                new Category { CategoryID = 4, CategoryName = "Drinks", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow, IsDeleted = false }
            );
        }
    }
}
