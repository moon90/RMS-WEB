using System;
using System.Collections.Generic;

namespace RMS.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public int BranchID { get; set; }
        public required string BranchName { get; set; }
        public string? BranchCode { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string CurrencyCode { get; set; } = "USD"; // ISO 4217 (e.g. USD, EUR, GBP)
        public string CurrencySymbol { get; set; } = "$";
        public bool IsMainBranch { get; set; } = false;

        // Navigation properties
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
        public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public virtual ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    }
}
