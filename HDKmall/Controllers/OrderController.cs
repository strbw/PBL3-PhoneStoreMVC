using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HDKmall.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public OrderController(IOrderService orderService, ICartService cartService, ICouponService couponService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _couponService = couponService;
        }

        // GET: Order/Checkout
        public IActionResult Checkout()
        {
            return View();
        }

        // POST: Order/CreateOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(string address, string paymentMethod, string couponCode = null)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
            {
                return Unauthorized();
            }

            var cart = _cartService.GetOrCreateCart(userId, null);
            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Giỏ hàng trống";
                return RedirectToAction("Index", "Cart");
            }

            decimal discountAmount = 0;
            if (!string.IsNullOrEmpty(couponCode))
            {
                var coupon = _couponService.ValidateCoupon(couponCode);
                if (coupon != null)
                {
                    discountAmount = coupon.DiscountAmount;
                }
            }

            var totalAmount = cart.Items.Sum(i => i.Product.Price * i.Quantity);
            var order = _orderService.CreateOrder(userId, cart.Items.ToList(), address, paymentMethod, totalAmount, discountAmount);

            // Clear cart
            _cartService.ClearCart(cart.Id);

            // Redirect to payment page
            TempData["Success"] = "Đơn hàng được tạo thành công. Vui lòng chọn phương thức thanh toán.";
            return RedirectToAction("Index", "Payment", new { orderId = order.Id });
        }

        // GET: Order/History
        public IActionResult History()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
            {
                return Unauthorized();
            }

            var orders = _orderService.GetUserOrders(userId);
            return View(orders);
        }

        // GET: Order/Detail/5
        public IActionResult Detail(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            // Check if user owns this order
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (order.UserId != userId && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            return View(order);
        }

        // POST: Order/Cancel
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (order.UserId != userId && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            if (order.Status == "Pending" || order.Status == "Processing")
            {
                _orderService.CancelOrder(id);
                TempData["Success"] = "Đơn hàng đã được huỷ";
            }
            else
            {
                TempData["Error"] = "Không thể huỷ đơn hàng ở trạng thái hiện tại";
            }

            return RedirectToAction("Detail", new { id });
        }
    }
}
