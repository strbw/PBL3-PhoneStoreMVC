using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HDKmall.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        // GET: Admin/Coupon
        public IActionResult Index()
        {
            ViewBag.ActiveTab = "coupons";
            var coupons = _couponService.GetAllCoupons();
            return View(coupons);
        }

        // GET: Admin/Coupon/Create
        public IActionResult Create()
        {
            ViewBag.ActiveTab = "coupons";
            return View(new Coupon { ExpiryDate = DateTime.Today.AddMonths(1), UsageLimit = 100 });
        }

        // POST: Admin/Coupon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveTab = "coupons";
                return View(coupon);
            }

            _couponService.CreateCoupon(coupon);
            TempData["Success"] = "Đã thêm mã giảm giá thành công.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Coupon/Edit/5
        public IActionResult Edit(int id)
        {
            ViewBag.ActiveTab = "coupons";
            var coupon = _couponService.GetAllCoupons().FirstOrDefault(c => c.Id == id);
            if (coupon == null) return NotFound();
            return View(coupon);
        }

        // POST: Admin/Coupon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveTab = "coupons";
                return View(coupon);
            }

            _couponService.UpdateCoupon(id, coupon);
            TempData["Success"] = "Đã cập nhật mã giảm giá.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Coupon/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _couponService.DeleteCoupon(id);
            TempData["Success"] = "Đã xóa mã giảm giá.";
            return RedirectToAction(nameof(Index));
        }
    }
}
