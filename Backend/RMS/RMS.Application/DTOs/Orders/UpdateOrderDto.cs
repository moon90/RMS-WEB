using System;
using System.Collections.Generic;
using RMS.Domain.Models.BaseModels; // For Result<T>

namespace RMS.Application.DTOs.Orders
{
    public class UpdateOrderDto
    {
        public int OrderID { get; set; } // Must include ID for update
        public DateTime OrderDate { get; set; }
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

        public List<UpdateOrderDetailDto> OrderDetails { get; set; }
    }
}