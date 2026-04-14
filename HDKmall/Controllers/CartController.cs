using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using HDKmall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HDKmall.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductSearchService _productSearchService;

        public CartController(ICartService cartService, IProductSearchService productSearchService)
        {
            _cartService = cartService;
            _productSearchService = productSearchService;
        }

        // GET: Cart
        public IActionResult Index()
        {
            var userId = GetUserId();
            var sessionId = GetSessionId();

            var cart = _cartService.GetOrCreateCart(userId, sessionId);
            if (cart == null || !cart.Items.Any())
            {
                return View(new ShoppingCart());
            }

            return View(cart);
        }

        // POST: Cart/Add
        [HttpPost]
        public IActionResult Add(int productId, int variantId = 0, int quantity = 1)
        {
            var userId = GetUserId();
            var sessionId = GetSessionId();

            var cart = _cartService.GetOrCreateCart(userId, sessionId);
            if (cart != null)
            {
                _cartService.AddToCart(cart.Id, productId, variantId, quantity);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng" });
                }

                return RedirectToAction(nameof(Index));
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = "Lỗi khi thêm sản phẩm" });
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/Remove
        [HttpPost]
        public IActionResult Remove(int cartItemId)
        {
            _cartService.RemoveFromCart(cartItemId);
            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/Update
        [HttpPost]
        public IActionResult Update(int cartItemId, int quantity)
        {
            if (quantity > 0)
            {
                _cartService.UpdateCartItem(cartItemId, quantity);
            }
            else
            {
                _cartService.RemoveFromCart(cartItemId);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/Clear
        [HttpPost]
        public IActionResult Clear()
        {
            var userId = GetUserId();
            var sessionId = GetSessionId();
            var cart = _cartService.GetOrCreateCart(userId, sessionId);

            if (cart != null)
            {
                _cartService.ClearCart(cart.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim?.Value, out var userId))
            {
                return userId;
            }
            return null;
        }

        private string GetSessionId()
        {
            if (!HttpContext.Session.TryGetValue("SessionId", out byte[] sessionIdBytes))
            {
                var sessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("SessionId", sessionId);
                return sessionId;
            }
            return System.Text.Encoding.UTF8.GetString(sessionIdBytes);
        }
    }
}
