using HDKmall.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HDKmall.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public IActionResult Index(string? q)
        {
            ViewBag.ActiveTab = "reviews";
            var reviews = _reviewService.GetAllReviews();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var key = q.Trim().ToLower();
                reviews = reviews.Where(r =>
                    (r.Product?.Name ?? "").ToLower().Contains(key) ||
                    (r.User?.FullName ?? "").ToLower().Contains(key) ||
                    (r.Comment ?? "").ToLower().Contains(key)
                );
            }

            ViewBag.Search = q;
            return View(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _reviewService.DeleteReview(id);
            TempData["Success"] = "Đã xóa đánh giá.";
            return RedirectToAction(nameof(Index));
        }
    }
}