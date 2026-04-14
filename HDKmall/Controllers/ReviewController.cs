using HDKmall.BLL.Interfaces;
using HDKmall.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        // POST: Review/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int productId, int rating, string comment)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
            {
                return Unauthorized();
            }

            // Check if user can review
            if (!_reviewService.UserCanReview(userId, productId))
            {
                return Json(new { success = false, message = "Bạn chỉ có thể đánh giá sản phẩm đã mua" });
            }

            var review = _reviewService.AddReview(productId, userId, rating, comment);
            return Json(new { success = true, message = "Đánh giá được thêm thành công" });
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
