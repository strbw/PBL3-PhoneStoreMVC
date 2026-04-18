using HDKmall.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            ViewBag.TotalOrders = allOrders.Count;
            ViewBag.TotalRevenue = allOrders
                .Where(o => o.Status == "Delivered" || o.Status == "Processing" || o.Status == "Shipped")
                .Sum(o => o.TotalAmount);
            ViewBag.TotalProducts = allProducts.Count;
            ViewBag.TotalUsers = allUsers.Count;

            ViewBag.OrdersByStatus = allOrders
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            ViewBag.RecentOrders = allOrders
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .ToList();

            return View();
        }
    }
}