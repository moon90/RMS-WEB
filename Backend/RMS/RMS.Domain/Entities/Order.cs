using System;
using System.Collections.Generic;

namespace RMS.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderTime { get; set; } // Consider TimeSpan or DateTime for better time handling
        public string TableName { get; set; } // Consider linking to DiningTable entity
        public string WaiterName { get; set; } // Consider linking to Staff entity
        public string OrderStatus { get; set; } // e.g., "Pending", "Preparing", "Served", "Paid", "Cancelled"
        public string OrderType { get; set; } // e.g., "DineIn", "TakeOut", "Delivery"
        public decimal Total { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int? PromotionID { get; set; } // Foreign Key to Promotions
        public decimal Received { get; set; }
        public decimal ChangeAmount { get; set; }
        public int? DriverID { get; set; } // Consider linking to Staff entity (for delivery drivers)
        public int? CustomerID { get; set; } // Foreign Key to Customers
        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }

        // Navigation property for OrderDetails
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}