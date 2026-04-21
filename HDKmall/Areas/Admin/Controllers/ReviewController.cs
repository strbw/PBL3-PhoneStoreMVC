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

        public IActionResult Index(string? q, string? status)
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

            if (status == "pending")
                reviews = reviews.Where(r => !r.IsApproved && !r.IsHidden);
            else if (status == "approved")
                reviews = reviews.Where(r => r.IsApproved && !r.IsHidden);
            else if (status == "hidden")
                reviews = reviews.Where(r => r.IsHidden);

            ViewBag.Search = q;
            ViewBag.Status = status;
            return View(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            _reviewService.ApproveReview(id);
            TempData["Success"] = "Đã duyệt đánh giá.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Hide(int id)
        {
            _reviewService.HideReview(id);
            TempData["Success"] = "Đã ẩn đánh giá.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Unhide(int id)
        {
            _reviewService.UnhideReview(id);
            TempData["Success"] = "Đã hiện lại đánh giá.";
            return RedirectToAction(nameof(Index));
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
