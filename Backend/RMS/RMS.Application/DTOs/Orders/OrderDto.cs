using System;
using System.Collections.Generic;
using RMS.Application.DTOs.CustomerDTOs.OutputDTOs; // Assuming CustomerDto exists

namespace RMS.Application.DTOs.Orders
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderTime { get; set; }
        public string TableName { get; set; }
        public string WaiterName { get; set; }
        public string OrderStatus { get; set; }
        public string OrderType { get; set; }
        public decimal Total { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int? PromotionID { get; set; }
        public decimal Received { get; set; }
        public decimal ChangeAmount { get; set; }
        public int? DriverID { get; set; }
        public int? CustomerID { get; set; }

        public CustomerDto Customer { get; set; } // Navigation property for Customer
        public ICollection<OrderDetailDto> OrderDetails { get; set; }
    }
}