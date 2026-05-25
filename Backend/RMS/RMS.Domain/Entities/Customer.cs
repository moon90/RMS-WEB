namespace RMS.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public int CustomerID { get; set; }
        public required string CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public string? Address { get; set; }
        public decimal TotalSpent { get; set; } = 0;
        public int LoyaltyPoints { get; set; } = 0;
        public string LoyaltyTier { get; set; } = "Bronze"; // Bronze, Silver, Gold, Platinum
        public DateTime? LastVisitDate { get; set; }
    }
}
