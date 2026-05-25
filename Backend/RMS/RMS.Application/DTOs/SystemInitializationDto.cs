using System;
using RMS.Application.Interfaces;

namespace RMS.Application.DTOs
{
    public class SystemInitializationDto
    {
        // Organization (Main Branch)
        public string OrganizationName { get; set; }
        public string? OrganizationCode { get; set; }
        public string? Address { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public string CurrencySymbol { get; set; } = "$";

        // Admin User
        public string AdminUserName { get; set; }
        public string AdminFullName { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }

        // White-Labeling
        public string? LogoBase64 { get; set; }
        public string PrimaryColor { get; set; } = "#4F46E5"; // Default Indigo
        public string SecondaryColor { get; set; } = "#DA291C"; // Default Red

        // System Settings
        public decimal DefaultTaxRate { get; set; } = 15;
        public decimal ServiceChargeRate { get; set; } = 5;
    }

    public class SystemStatusDto
    {
        public bool IsInitialized { get; set; }
        public string? OrganizationName { get; set; }
    }
}
