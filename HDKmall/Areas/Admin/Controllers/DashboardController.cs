using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using HDKmall.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HDKmall.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;

        public DashboardController(IOrderService orderService, IProductService productService, IAccountService accountService)
        {
            _orderService = orderService;
            _productService = productService;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            ViewBag.ActiveTab = "dashboard";

            var allOrders = _orderService.GetAllOrders().ToList();
            var allProducts = _productService.GetAllProducts().ToList();
            var allUsers = _accountService.GetAllUsers().ToList();

            var viewModel = new DashboardViewModel
            {
                TotalOrders = allOrders.Count,
                TotalRevenue = allOrders
                    .Where(o => o.Status == "Delivered" || o.Status == "Processing" || o.Status == "Shipping")
                    .Sum(o => o.TotalAmount),
                TotalProducts = allProducts.Count,
                TotalUsers = allUsers.Count,
                RecentOrders = allOrders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToList()
            };

            // 1. Monthly Revenue (Last 6 months)
            var last6Months = Enumerable.Range(0, 6)
                .Select(i => DateTime.Today.AddMonths(-i))
                .Select(d => new { Month = d.Month, Year = d.Year })
                .Reverse()
                .ToList();

            viewModel.MonthlyRevenueLabels = last6Months.Select(m => $"{m.Month}/{m.Year}").ToList();
            viewModel.MonthlyRevenueData = last6Months.Select(m => 
                allOrders.Where(o => o.OrderDate.Month == m.Month && o.OrderDate.Year == m.Year && (o.Status == "Delivered" || o.Status == "Processing" || o.Status == "Shipping"))
                         .Sum(o => o.TotalAmount)).ToList();

            // 2. Daily Orders (Last 7 days)
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-i))
                .Reverse()
                .ToList();

            viewModel.DailyOrderLabels = last7Days.Select(d => d.ToString("dd/MM")).ToList();
            viewModel.DailyOrderData = last7Days.Select(d => 
                allOrders.Count(o => o.OrderDate.Date == d.Date)).ToList();

            // 3. Top Products (By Units Sold)
            viewModel.TopProducts = allOrders
                .Where(o => o.Status != "Cancelled" && o.Status != "Failed")
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => new { od.ProductId, od.Product?.Name })
                .Select(g => new TopProductItem { 
                    ProductName = g.Key.Name ?? "Unknown", 
                    Quantity = g.Sum(od => od.Quantity),
                    Revenue = g.Sum(od => od.Quantity * od.UnitPrice)
                })
                .OrderByDescending(g => g.Quantity)
                .Take(5)
                .ToList();

            // 4. Low Stock Products
            viewModel.LowStockProducts = allProducts
                .SelectMany(p => (p.Versions ?? new List<ProductVersion>())
                    .SelectMany(v => (v.Variants ?? new List<ProductVariant>())
                        .Select(vr => new LowStockItem { 
                            ProductName = p.Name, 
                            VariantName = $"{vr.Color} {v.Name}", 
                            Stock = vr.Stock 
                        })))
                .Where(v => v.Stock < 10)
                .OrderBy(v => v.Stock)
                .Take(5)
                .ToList();

            // 5. Orders By Status
            viewModel.OrdersByStatus = allOrders
                .GroupBy(o => o.Status ?? "Chờ xử lý")
                .Select(g => new OrderStatusItem { Status = g.Key, Count = g.Count() })
                .ToList();

            return View(viewModel);
        }
    }
}
