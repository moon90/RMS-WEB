using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;

namespace RMS.Infrastructure.Configurations
{
    public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
    {
        public void Configure(EntityTypeBuilder<SystemSetting> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SettingKey)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(s => s.SettingKey).IsUnique();

            builder.Property(s => s.SettingValue); // Defaults to nvarchar(max) in SQL Server or similar large text type

            builder.Property(s => s.SettingGroup)
                .HasMaxLength(50);

            // Seed Data
            builder.HasData(
                // General
                new SystemSetting { Id = 1, SettingKey = "RestaurantName", SettingValue = "RMS Restaurant", SettingGroup = "General", SettingType = "String", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 4, SettingKey = "PageTitle", SettingValue = "RMS POS System", SettingGroup = "General", SettingType = "String", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 7, SettingKey = "RestaurantAddress", SettingValue = "123 Foodie Street, Gourmet City", SettingGroup = "General", SettingType = "String", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 8, SettingKey = "RestaurantPhone", SettingValue = "+1 234 567 890", SettingGroup = "General", SettingType = "String", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 9, SettingKey = "RestaurantEmail", SettingValue = "info@rms-restaurant.com", SettingGroup = "General", SettingType = "String", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 10, SettingKey = "VATNumber", SettingValue = "VAT-987654321", SettingGroup = "General", SettingType = "String", CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                // Localization
                new SystemSetting { Id = 2, SettingKey = "CurrencySymbol", SettingValue = "$", SettingGroup = "Localization", SettingType = "String", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 11, SettingKey = "TimeZone", SettingValue = "Central European Time (CET)", SettingGroup = "Localization", SettingType = "String", CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                // Appearance
                new SystemSetting { Id = 3, SettingKey = "RestaurantLogo", SettingValue = "", SettingGroup = "Appearance", SettingType = "Image", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 5, SettingKey = "Favicon", SettingValue = "", SettingGroup = "Appearance", SettingType = "Image", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 12, SettingKey = "PrimaryColor", SettingValue = "#1e40af", SettingGroup = "Appearance", SettingType = "Color", CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                // POS Settings
                new SystemSetting { Id = 6, SettingKey = "DefaultTaxRate", SettingValue = "15", SettingGroup = "POS", SettingType = "Number", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 13, SettingKey = "ServiceChargeRate", SettingValue = "5", SettingGroup = "POS", SettingType = "Number", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 14, SettingKey = "AutoPrintKOT", SettingValue = "true", SettingGroup = "POS", SettingType = "Boolean", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 15, SettingKey = "AutoPrintReceipt", SettingValue = "true", SettingGroup = "POS", SettingType = "Boolean", CreatedBy = "system", CreatedDate = DateTime.UtcNow },

                // Receipt Customization
                new SystemSetting { Id = 16, SettingKey = "ReceiptHeaderNote", SettingValue = "Welcome to RMS Restaurant!", SettingGroup = "Receipt", SettingType = "String", CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new SystemSetting { Id = 17, SettingKey = "ReceiptFooterNote", SettingValue = "Thank you for dining with us!", SettingGroup = "Receipt", SettingType = "String", CreatedBy = "system", CreatedDate = DateTime.UtcNow }
            );
        }
    }
}
