using System.Security.Claims;
using System.Threading.Tasks;
using HDKmall.BLL.Interfaces;
using HDKmall.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HDKmall.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddReviewVM vm)
        {
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu form không hợp lệ, đẩy về lại trang Chi tiết sản phẩm
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin đánh giá.";
                return RedirectToAction("Detail", "Product", new { id = vm.ProductId });
            }

            // Lấy UserId của người đang đăng nhập. 
            // Lưu ý: Tùy vào cách nhóm bạn cấu hình đăng nhập, cách lấy ID có thể khác.
            // Nếu bạn dùng Session, hãy sửa lại thành: int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (int.TryParse(userIdClaim, out int userId))
            {
                // Kiểm tra lại quyền đánh giá (đã mua hàng & chưa đánh giá sản phẩm này)
                if (!_reviewService.UserCanReview(userId, vm.ProductId))
                {
                    TempData["Error"] = "Bạn không thể đánh giá sản phẩm này (có thể bạn chưa mua hoặc đã đánh giá rồi).";
                    return RedirectToAction("Detail", "Product", new { id = vm.ProductId });
                }

                // Chuyển toàn bộ dữ liệu (bao gồm cả ảnh) xuống Service xử lý
                await _reviewService.AddReviewAsync(vm, userId);
                TempData["Success"] = "Cảm ơn bạn! Đánh giá của bạn đã được hiển thị trên hệ thống.";
            }

            return RedirectToAction("Detail", "Product", new { id = vm.ProductId });
        }

        // POST: Review/Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            _reviewService.DeleteReview(id);
            TempData["Success"] = "Đánh giá đã được xoá";
            return RedirectToRequest();
        }

        private IActionResult RedirectToRequest()
        {
            return Redirect(Request.Headers["Referer"].ToString() ?? "/");
        }
    }
}
