using System.Collections.Generic;

namespace HDKmall.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }

        public List<string> MonthlyRevenueLabels { get; set; } = new List<string>();
        public List<decimal> MonthlyRevenueData { get; set; } = new List<decimal>();

        public List<string> DailyOrderLabels { get; set; } = new List<string>();
        public List<int> DailyOrderData { get; set; } = new List<int>();

        public List<TopProductItem> TopProducts { get; set; } = new List<TopProductItem>();
        public List<LowStockItem> LowStockProducts { get; set; } = new List<LowStockItem>();
        public List<OrderStatusItem> OrdersByStatus { get; set; } = new List<OrderStatusItem>();
        public List<HDKmall.Models.Order> RecentOrders { get; set; } = new List<HDKmall.Models.Order>();
    }

    public class TopProductItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }

    public class LowStockItem
    {
        public string ProductName { get; set; }
        public string VariantName { get; set; }
        public int Stock { get; set; }
    }

    public class OrderStatusItem
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
