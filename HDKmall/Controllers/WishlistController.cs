using HDKmall.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HDKmall.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var wishlist = await _wishlistService.GetUserWishlistAsync(userId);
                return View(wishlist);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Toggle(int productId, int? versionId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để sử dụng tính năng này" });
            }

            var isAdded = await _wishlistService.ToggleWishlistAsync(userId, productId, versionId);
            var count = await _wishlistService.GetWishlistCountAsync(userId);

            return Json(new { 
                success = true, 
                isAdded = isAdded, 
                count = count,
                message = isAdded ? "Đã thêm vào danh sách yêu thích" : "Đã xóa khỏi danh sách yêu thích" 
            });
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCount()
        {
            if (!User.Identity.IsAuthenticated) return Json(0);
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var count = await _wishlistService.GetWishlistCountAsync(userId);
                return Json(count);
            }
            return Json(0);
        }
    }
}
