using System;
using RMS.Application.Interfaces;

namespace RMS.Application.DTOs
{
    public class BranchDto
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string? BranchCode { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsMainBranch { get; set; }
    }

    public class CreateBranchDto
    {
        public string BranchName { get; set; }
        public string? BranchCode { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public string CurrencySymbol { get; set; } = "$";
    }
}
