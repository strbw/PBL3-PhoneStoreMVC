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
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0) return RedirectToAction("Login", "Account");

            var cart = _cartService.GetOrCreateCart(userId, null);
            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Giỏ hàng trống";
                return RedirectToAction("Index", "Cart");
            }

            // Get selected item IDs from TempData (set by Cart/ProceedToCheckout)
            List<int> selectedIds = null;
            if (TempData["SelectedCartItems"] is string selectedStr && !string.IsNullOrEmpty(selectedStr))
            {
                selectedIds = selectedStr.Split(',').Select(int.Parse).ToList();
                // Keep TempData for the POST
                TempData.Keep("SelectedCartItems");
            }

            // Filter to selected items, or use all items if none selected
            ShoppingCart checkoutCart;
            if (selectedIds != null && selectedIds.Any())
            {
                checkoutCart = new ShoppingCart
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    Items = cart.Items.Where(i => selectedIds.Contains(i.Id)).ToList()
                };
            }
            else
            {
                checkoutCart = cart;
            }

            return View(checkoutCart);
        }

        // POST: Order/CreateOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(string address, string city, string district, string paymentMethod, string couponCode = null, string note = null, decimal shippingFee = 0)
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

            // Get selected item IDs
            List<int> selectedIds = null;
            if (TempData["SelectedCartItems"] is string selectedStr && !string.IsNullOrEmpty(selectedStr))
            {
                selectedIds = selectedStr.Split(',').Select(int.Parse).ToList();
            }

            var itemsToOrder = selectedIds != null && selectedIds.Any()
                ? cart.Items.Where(i => selectedIds.Contains(i.Id)).ToList()
                : cart.Items.ToList();

            if (!itemsToOrder.Any())
            {
                TempData["Error"] = "Không có sản phẩm nào được chọn";
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

            var fullAddress = string.IsNullOrEmpty(district)
                ? $"{address}, {city}"
                : $"{address}, {district}, {city}";

            var subTotal = itemsToOrder.Sum(i =>
                (i.Variant != null ? i.Variant.Price : i.Product?.Price ?? 0) * i.Quantity);
            var totalAmount = subTotal + shippingFee - discountAmount;

            var order = _orderService.CreateOrder(userId, itemsToOrder, fullAddress, paymentMethod, totalAmount, 0);

            // Remove only the ordered items from cart
            _cartService.RemoveFromCart(itemsToOrder.Select(i => i.Id).ToList());

            TempData["Success"] = "Đơn hàng đã được tạo thành công!";
            return RedirectToAction("Detail", new { id = order.Id });
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
