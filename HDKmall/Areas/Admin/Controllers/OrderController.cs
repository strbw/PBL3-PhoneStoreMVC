using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HDKmall.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index(string? q, string? status)
        {
            ViewBag.ActiveTab = "orders";
            var orders = _orderService.GetAllOrders();

            if (!string.IsNullOrWhiteSpace(status))
            {
                orders = orders.Where(o => o.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                var key = q.Trim().ToLower();
                orders = orders.Where(o =>
                    o.Id.ToString().Contains(key) ||
                    (o.User?.FullName ?? "").ToLower().Contains(key) ||
                    (o.User?.Email ?? "").ToLower().Contains(key) ||
                    (o.Address ?? "").ToLower().Contains(key) ||
                    (o.Status ?? "").ToLower().Contains(key)
                );
            }

            ViewBag.Search = q;
            ViewBag.SelectedStatus = status;
            return View(orders);
        }

        public IActionResult Detail(int id)
        {
            ViewBag.ActiveTab = "orders";
            var order = _orderService.GetOrderById(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null) return NotFound();

            _orderService.UpdateOrderStatus(id, status);
            TempData["Success"] = $"Đã cập nhật trạng thái đơn hàng #{id} thành \"{status}\".";
            return RedirectToAction(nameof(Index));
        }
    }
}