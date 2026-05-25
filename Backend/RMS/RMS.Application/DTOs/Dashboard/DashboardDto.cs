using System;
using System.Collections.Generic;
using RMS.Application.Interfaces;

namespace RMS.Application.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal RevenueGrowth { get; set; } // Percentage
        public decimal OrdersGrowth { get; set; } // Percentage
        public double InventoryHealthPercentage { get; set; }
        public int LowStockItemsCount { get; set; }
    }

    public class SalesTrendDto
    {
        public string Label { get; set; } // Time (e.g., "08:00") or Date
        public decimal Amount { get; set; }
    }

    public class TrendingMenuItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Margin => Price - CostPrice;
        public int OrderCount { get; set; }
        public string? ThumbnailImage { get; set; }
    }

    public class AiInsightDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // Success, Warning, Info
        public string Impact { get; set; } // Low, Medium, High
    }

    public class StaffPerformanceDto
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public string Role { get; set; }
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
        public decimal AverageOrderValue => OrderCount == 0 ? 0 : TotalRevenue / OrderCount;
        public string PerformanceTag { get; set; } // e.g. "Top Performer", "Rising Star"
        public string? BranchName { get; set; }
    }

    public class CustomerLoyaltyDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal LifetimeValue { get; set; }
        public int Points { get; set; }
        public string Tier { get; set; }
        public string? LastVisit { get; set; }
    }

    public class KitchenProductivityDto
    {
        public int ChefId { get; set; }
        public string ChefName { get; set; }
        public double AveragePrepTimeMinutes { get; set; }
        public int OrdersCompleted { get; set; }
        public string EfficiencyTag { get; set; } // e.g. "Fastest", "Steady", "Training Needed"
    }

    public class DashboardDto
    {
        public DashboardStatsDto Stats { get; set; }
        public List<SalesTrendDto> RevenueTrend { get; set; }
        public List<TrendingMenuItemDto> TrendingMenus { get; set; }
        public List<AiInsightDto> AiInsights { get; set; }
        public List<StaffPerformanceDto> StaffPerformance { get; set; }
        public List<CustomerLoyaltyDto> TopCustomers { get; set; }
        public List<KitchenProductivityDto> KitchenProductivity { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public string CurrencySymbol { get; set; } = "$";
    }
}
