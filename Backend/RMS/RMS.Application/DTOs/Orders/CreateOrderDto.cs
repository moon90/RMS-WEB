using System;
using System.Collections.Generic;
using RMS.Domain.Models.BaseModels; // For Result<T>

namespace RMS.Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        public DateTime OrderDate { get; set; }
        public string OrderTime { get; set; }
        public string TableName { get; set; }
        public string WaiterName { get; set; }
        public string OrderStatus { get; set; } = "Pending"; // Default status
        public string OrderType { get; set; } // e.g., DineIn, TakeOut, Delivery
        public decimal Total { get; set; }
        public decimal DiscountAmount { get; set; } = 0m;
        public decimal DiscountPercentage { get; set; } = 0m;
        public int? PromotionID { get; set; }
        public decimal Received { get; set; } = 0m;
        public decimal ChangeAmount { get; set; } = 0m;
        public int? DriverID { get; set; }
        public int? CustomerID { get; set; }

        public List<CreateOrderDetailDto> OrderDetails { get; set; }
    }
}