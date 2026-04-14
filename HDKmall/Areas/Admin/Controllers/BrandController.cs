using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HDKmall.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET: Admin/Brand
        public IActionResult Index()
        {
            var brands = _brandService.GetAllBrands();
            return View(brands);
        }

        // GET: Admin/Brand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brand/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                var result = await _brandService.AddBrandAsync(brand);
                if (result)
                {
                    TempData["Success"] = "Thương hiệu được tạo thành công";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(brand);
        }

        // GET: Admin/Brand/Edit/5
        public IActionResult Edit(int id)
        {
            var brand = _brandService.GetBrandById(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Admin/Brand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _brandService.UpdateBrandAsync(id, brand);
                if (result)
                {
                    TempData["Success"] = "Thương hiệu được cập nhật thành công";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(brand);
        }

        // POST: Admin/Brand/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _brandService.DeleteBrandAsync(id);
            if (result)
            {
                TempData["Success"] = "Thương hiệu được xoá thành công";
            }
            else
            {
                TempData["Error"] = "Lỗi khi xoá thương hiệu";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
