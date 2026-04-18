using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HDKmall.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService;
        private readonly IPhotoService _photoService;

        public BannerController(IBannerService bannerService, IPhotoService photoService)
        {
            _bannerService = bannerService;
            _photoService = photoService;
        }

        public IActionResult Index(string? q)
        {
            ViewBag.ActiveTab = "banners";
            var banners = _bannerService.GetAllBanners();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var key = q.Trim().ToLower();
                banners = banners.Where(b => (b.Title ?? "").ToLower().Contains(key));
            }

            ViewBag.Search = q;
            return View(banners);
        }

        public IActionResult Create()
        {
            ViewBag.ActiveTab = "banners";
            return View(new Banner { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner banner)
        {
            if (banner.ImageFile != null && banner.ImageFile.Length > 0)
            {
                var upload = await _photoService.AddPhotoAsync(banner.ImageFile);
                if (upload.Error == null) banner.ImageUrl = upload.SecureUrl.AbsoluteUri;
            }

            if (string.IsNullOrWhiteSpace(banner.ImageUrl))
            {
                ModelState.AddModelError("ImageUrl", "Vui lòng chọn ảnh hoặc nhập URL ảnh.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ActiveTab = "banners";
                return View(banner);
            }

            _bannerService.CreateBanner(banner);
            TempData["Success"] = "Đã thêm banner thành công.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            ViewBag.ActiveTab = "banners";
            var banner = _bannerService.GetBannerById(id);
            if (banner == null) return NotFound();
            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Banner banner)
        {
            if (banner.ImageFile != null && banner.ImageFile.Length > 0)
            {
                var upload = await _photoService.AddPhotoAsync(banner.ImageFile);
                if (upload.Error == null) banner.ImageUrl = upload.SecureUrl.AbsoluteUri;
            }

            if (string.IsNullOrWhiteSpace(banner.ImageUrl))
            {
                ModelState.AddModelError("ImageUrl", "Vui lòng chọn ảnh hoặc nhập URL ảnh.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ActiveTab = "banners";
                return View(banner);
            }

            _bannerService.UpdateBanner(id, banner);
            TempData["Success"] = "Đã cập nhật banner.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _bannerService.DeleteBanner(id);
            TempData["Success"] = "Đã xóa banner.";
            return RedirectToAction(nameof(Index));
        }
    }
}