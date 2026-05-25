using System;
using System.Collections.Generic;
using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderTime { get; set; } // Consider TimeSpan or DateTime for better time handling
        public string? TableName { get; set; } // Consider linking to DiningTable entity
        public string? WaiterName { get; set; } 
        public int? StaffID { get; set; } // Linked Staff ID (Waiter)
        public virtual Staff? Waiter { get; set; }

        public int? ChefID { get; set; } // Linked Staff ID (Chef)
        public virtual Staff? Chef { get; set; }

        public DateTime? PreparationStart { get; set; }
        public DateTime? PreparationEnd { get; set; }

        public string OrderStatus { get; set; } // e.g., "Pending", "Preparing", "Served", "Paid", "Cancelled"
        public string OrderType { get; set; } // e.g., "DineIn", "TakeOut", "Delivery"
        public decimal Total { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ServiceChargeAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int? PromotionID { get; set; } // Foreign Key to Promotions
        public decimal Received { get; set; }
        public decimal ChangeAmount { get; set; }
        public int? DriverID { get; set; } // Consider linking to Staff entity (for delivery drivers)
        public int? CustomerID { get; set; } // Foreign Key to Customers
        public virtual Customer? Customer { get; set; } // Navigation property for Customer
        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        
        // New Additions
        public string? TokenNumber { get; set; } // Token for Queue System
        public decimal TipAmount { get; set; }   // Track gratuity/tips
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }

        // Navigation property for OrderDetails
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}