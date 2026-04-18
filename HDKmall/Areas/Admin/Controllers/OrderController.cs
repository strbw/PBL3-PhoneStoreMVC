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

        // GET: Admin/Order
        public IActionResult Index()
        {
            ViewBag.ActiveTab = "orders";
            var orders = _orderService.GetAllOrders();
            return View(orders);
        }

        // POST: Admin/Order/UpdateStatus
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
