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
        public async Task<IActionResult> Add(int productId, int rating, string comment, List<IFormFile> images, List<int> tagIds)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
            {
                return Unauthorized();
            }

            // Check if user can review (must have purchased with Paid status)
            if (!_reviewService.UserCanReview(userId, productId))
            {
                TempData["ReviewError"] = "Bạn chỉ có thể đánh giá sản phẩm đã mua và đơn hàng đã thanh toán.";
                return Redirect($"/Product/Detail/{productId}");
            }

            if (rating < 1 || rating > 5)
            {
                TempData["ReviewError"] = "Vui lòng chọn số sao (1-5).";
                return Redirect($"/Product/Detail/{productId}");
            }

            await _reviewService.AddReviewAsync(productId, userId, rating, comment, images, tagIds);
            TempData["ReviewSuccess"] = "Đánh giá của bạn đã được gửi và đang chờ duyệt.";
            return Redirect($"/Product/Detail/{productId}");
        }

        // POST: Review/Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
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
