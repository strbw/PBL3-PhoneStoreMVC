using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
using HDKmall.BLL.Interfaces;
using HDKmall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HDKmall.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductSearchService _searchService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IReviewService _reviewService;
        private readonly HDKmall.DAL.Interfaces.IProductRepository _productRepo;

        public ProductController(IProductSearchService searchService, ICategoryService categoryService, IBrandService brandService, IReviewService reviewService, HDKmall.DAL.Interfaces.IProductRepository productRepo)
        {
            _searchService = searchService;
            _categoryService = categoryService;
            _brandService = brandService;
            _reviewService = reviewService;
            _productRepo = productRepo;
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC).Replace("đ", "d").Replace("Đ", "D");
        }

        // GET: Product/Index
        public IActionResult Index(int? categoryId, string cat, int? brandId, string brand, decimal? minPrice, decimal? maxPrice, string sortBy = "newest", int page = 1)
        {
            var allCategories = _categoryService.GetAllCategories();
            var allBrands = _brandService.GetAllBrands();

            if (!string.IsNullOrEmpty(cat) && !categoryId.HasValue)
            {
                var category = allCategories.FirstOrDefault(c => RemoveDiacritics(c.Name).ToLower().Replace(" ", "-") == cat.ToLower());
                if (category != null)
                {
                    categoryId = category.Id;
                }
            }

            if (!string.IsNullOrEmpty(brand) && !brandId.HasValue)
            {
                var br = allBrands.FirstOrDefault(b => RemoveDiacritics(b.Name).ToLower().Replace(" ", "-") == brand.ToLower());
                if (br != null)
                {
                    brandId = br.Id;
                }
            }

            var filter = new ProductFilterVM
            {
                CategoryId = categoryId,
                BrandId = brandId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortBy = sortBy,
                PageNumber = page,
                PageSize = 12
            };

            var result = _searchService.SearchProducts(filter);

            // Pass filter info to view for sidebar
            ViewBag.Categories = allCategories;
            ViewBag.Brands = allBrands;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedBrandId = brandId;
            ViewBag.SelectedMinPrice = minPrice;
            ViewBag.SelectedMaxPrice = maxPrice;
            ViewBag.SelectedSortBy = sortBy;

            return View(result);
        }

        // GET: Product/Detail/5 or /product/slug
        [Route("Product/Detail/{id:int}")]
        [Route("product/{slug}")]
        public IActionResult Detail(int? id, string slug)
        {
            ProductDetailVM product = null;

            if (!string.IsNullOrEmpty(slug))
            {
                product = _searchService.GetProductDetailBySlug(slug);
                
                // Fallback: Nếu không tìm thấy theo slug trong DB (do chưa update dữ liệu cũ)
                if (product == null)
                {
                    var allProducts = _productRepo.GetAll();
                    var match = allProducts.FirstOrDefault(p => HDKmall.Helpers.SlugHelper.GenerateSlug(p.Name) == slug);
                    if (match != null)
                    {
                        match.Slug = slug;
                        _productRepo.Update(match);
                        product = _searchService.GetProductDetail(match.Id);
                    }
                }
            }
            else if (id.HasValue)
            {
                product = _searchService.GetProductDetail(id.Value);
                // Tự động cập nhật slug nếu còn thiếu để lần sau link đẹp hơn
                if (product != null && string.IsNullOrEmpty(product.Slug))
                {
                    var p = _productRepo.GetById(product.Id);
                    if (p != null)
                    {
                        p.Slug = HDKmall.Helpers.SlugHelper.GenerateSlug(p.Name);
                        _productRepo.Update(p);
                    }
                }
            }

            if (product == null)
            {
                return NotFound();
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdString, out var userId) && userId > 0)
                {
                    ViewBag.CanReview = _reviewService.UserCanReview(userId, product.Id);
                }
                else
                {
                    ViewBag.CanReview = false;
                }
            }
            else
            {
                ViewBag.CanReview = false;
            }

            return View(product);
        }

        // GET: Product/Search?q=iphone
        public IActionResult Search(string q, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return RedirectToAction("Index");
            }

            var filter = new ProductFilterVM
            {
                SearchQuery = q,
                PageNumber = page,
                PageSize = 12
            };

            var result = _searchService.SearchProducts(filter);
            ViewBag.SearchQuery = q;

            return View("Index", result);
        }

        // GET: Product/Category/1
        public IActionResult Category(int id, int page = 1)
        {
            var filter = new ProductFilterVM
            {
                CategoryId = id,
                PageNumber = page,
                PageSize = 12
            };

            var result = _searchService.SearchProducts(filter);
            ViewBag.Categories = _categoryService.GetAllCategories();
            ViewBag.SelectedCategoryId = id;

            return View("Index", result);
        }

        // GET: Product/Brand/1
        public IActionResult Brand(int id, int page = 1)
        {
            var filter = new ProductFilterVM
            {
                BrandId = id,
                PageNumber = page,
                PageSize = 12
            };

            var result = _searchService.SearchProducts(filter);
            ViewBag.Brands = _brandService.GetAllBrands();
            ViewBag.SelectedBrandId = id;

            return View("Index", result);
        }

        // API Endpoints for Home Page
        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetBrands()
        {
            try
            {
                var brands = _brandService.GetAllBrands();
                var brandNames = brands.Select(b => b.Name).ToList();
                return Ok(brandNames);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                var filter = new ProductFilterVM { PageSize = 100 };
                var result = _searchService.SearchProducts(filter);
                return Ok(result.Products);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("Product/UpdateAllSlugs")]
        public IActionResult UpdateAllSlugs()
        {
            var products = _productRepo.GetAll();
            int count = 0;
            foreach (var p in products)
            {
                if (string.IsNullOrEmpty(p.Slug))
                {
                    p.Slug = HDKmall.Helpers.SlugHelper.GenerateSlug(p.Name);
                    _productRepo.Update(p);
                    count++;
                }
            }
            TempData["success"] = $"Đã cập nhật thành công {count} sản phẩm!";
            return RedirectToAction("Index");
        }
    }
}
