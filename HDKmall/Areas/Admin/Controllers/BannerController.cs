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

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        // GET: Admin/Banner
        public IActionResult Index()
        {
            ViewBag.ActiveTab = "banners";
            var banners = _bannerService.GetAllBanners();
            return View(banners);
        }

        // GET: Admin/Banner/Create
        public IActionResult Create()
        {
            ViewBag.ActiveTab = "banners";
            return View(new Banner { IsActive = true });
        }

        // POST: Admin/Banner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Banner banner)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveTab = "banners";
                return View(banner);
            }

            _bannerService.CreateBanner(banner);
            TempData["Success"] = "Đã thêm banner thành công.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Banner/Edit/5
        public IActionResult Edit(int id)
        {
            ViewBag.ActiveTab = "banners";
            var banner = _bannerService.GetBannerById(id);
            if (banner == null) return NotFound();
            return View(banner);
        }

        // POST: Admin/Banner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Banner banner)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveTab = "banners";
                return View(banner);
            }

            _bannerService.UpdateBanner(id, banner);
            TempData["Success"] = "Đã cập nhật banner.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Banner/Delete/5
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
