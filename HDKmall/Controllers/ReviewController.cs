using System.Security.Claims;
using System.Threading.Tasks;
using HDKmall.BLL.Interfaces;
using HDKmall.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HDKmall.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly HDKmall.DAL.Interfaces.IProductRepository _productRepo;

        public ReviewController(IReviewService reviewService, HDKmall.DAL.Interfaces.IProductRepository productRepo)
        {
            _reviewService = reviewService;
            _productRepo = productRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddReviewVM vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin đánh giá.";
                return RedirectToRequest();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (int.TryParse(userIdClaim, out int userId))
            {
                if (!_reviewService.UserCanReview(userId, vm.ProductVersionId))
                {
                    TempData["Error"] = "Bạn không thể đánh giá sản phẩm này.";
                    return RedirectToRequest();
                }

                await _reviewService.AddReviewAsync(vm, userId);
                TempData["Success"] = "Cảm ơn bạn! Đánh giá của bạn đã được gửi.";
            }

            return RedirectToRequest();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _reviewService.DeleteReview(id);
            TempData["Success"] = "Đánh giá đã được xoá";
            return RedirectToRequest();
        }

        private IActionResult RedirectToRequest()
        {
            var referer = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer)) return RedirectToAction("Index", "Home");
            return Redirect(referer);
        }
    }
}
