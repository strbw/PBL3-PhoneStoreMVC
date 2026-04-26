using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HDKmall.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Admin/Category
        public IActionResult Index()
        {
            ViewBag.ActiveTab = "categories";
            var categories = _categoryService.GetAllCategories();
            return View(categories);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.AddCategoryAsync(category);
                if (result)
                {
                    TempData["Success"] = "Danh mục được tạo thành công";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateCategoryAsync(id, category);
                if (result)
                {
                    TempData["Success"] = "Danh mục được cập nhật thành công";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result)
            {
                TempData["Success"] = "Danh mục được xoá thành công";
            }
            else
            {
                TempData["Error"] = "Lỗi khi xoá danh mục";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
