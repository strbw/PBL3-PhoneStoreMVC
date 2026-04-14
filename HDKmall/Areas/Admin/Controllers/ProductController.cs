using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using HDKmall.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HDKmall.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        public ProductController(IProductService productService, ICategoryService categoryService, IBrandService brandService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
        }

        private void PopulateDropdowns(int? selectedCategoryId = null, int? selectedBrandId = null)
        {
            var categories = _categoryService.GetAllCategories();
            var brands = _brandService.GetAllBrands();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategoryId);
            ViewBag.Brands = new SelectList(brands, "Id", "Name", selectedBrandId);
        }

        public IActionResult Index()
        {
            ViewBag.ActiveTab = "products";
            var products = _productService.GetAllProducts();
            return View(products);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewBag.ActiveTab = "products";
            PopulateDropdowns();
            return View(new ProductVM());
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.AddProductAsync(vm);
                if (result)
                {
                    TempData["Success"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Lỗi khi lưu sản phẩm. Vui lòng kiểm tra lại.");
            }
            else
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                ModelState.AddModelError("", "Lỗi nhập liệu: " + errors);
            }
            ViewBag.ActiveTab = "products";
            PopulateDropdowns(vm.CategoryId, vm.BrandId);
            return View(vm);
        }

        // GET: Admin/Product/Edit/5
        public IActionResult Edit(int id)
        {
            ViewBag.ActiveTab = "products";
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            var vm = new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                ImageUrl = product.ImageUrl,
                Variants = (product.Variants ?? new List<ProductVariant>()).Select(v => new ProductVariantVM
                {
                    Id = v.Id,
                    ProductId = v.ProductId,
                    Color = v.Color ?? "",
                    Capacity = v.Capacity ?? "",
                    Price = v.Price,
                    Stock = v.Stock,
                    ImageUrl = v.ImageUrl ?? ""
                }).ToList(),
                Specifications = (product.Specifications ?? new List<ProductSpecification>())
                    .OrderBy(s => s.DisplayOrder)
                    .Select(s => new ProductSpecVM
                    {
                        Id = s.Id,
                        SpecName = s.SpecName,
                        SpecValue = s.SpecValue,
                        DisplayOrder = s.DisplayOrder
                    }).ToList()
            };

            PopulateDropdowns(vm.CategoryId, vm.BrandId);
            return View(vm);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductVM vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProductAsync(id, vm);
                if (result)
                {
                    TempData["Success"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật sản phẩm!");
            }
            else
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                ModelState.AddModelError("", "Lỗi nhập liệu: " + errors);
            }
            ViewBag.ActiveTab = "products";
            PopulateDropdowns(vm.CategoryId, vm.BrandId);
            return View(vm);
        }

        // GET: Admin/Product/Delete/5
        public IActionResult Delete(int id)
        {
            ViewBag.ActiveTab = "products";
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result)
                TempData["Success"] = "Đã xóa sản phẩm thành công!";
            else
                TempData["Error"] = "Có lỗi xảy ra khi xóa sản phẩm!";

            return RedirectToAction(nameof(Index));
        }
    }
}

