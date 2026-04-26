using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HDKmall.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductSearchService _searchService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IProductSearchService searchService, ICategoryService categoryService, IBrandService brandService, ILogger<HomeController> logger)
        {
            _searchService = searchService;
            _categoryService = categoryService;
            _brandService = brandService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var featuredProducts = _searchService.GetFeaturedProducts(8);
            var newProducts = _searchService.GetNewProducts(8);

            ViewBag.Categories = _categoryService.GetAllCategories();
            ViewBag.Brands = _brandService.GetAllBrands();
            ViewBag.FeaturedProducts = featuredProducts;
            ViewBag.NewProducts = newProducts;

            if (User.Identity.IsAuthenticated)
            {
                var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdStr, out int userId))
                {
                    var wishlistService = HttpContext.RequestServices.GetService<IWishlistService>();
                    if (wishlistService != null)
                    {
                        var wishlist = await wishlistService.GetUserWishlistAsync(userId);
                        ViewBag.WishlistProductIds = wishlist.Select(w => w.Id).ToList();
                    }
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode == 404)
            {
                ViewData["ErrorTitle"] = "404 - Không tìm thấy trang";
                ViewData["ErrorMessage"] = "Rất tiếc, trang bạn đang tìm kiếm không tồn tại hoặc đã bị di chuyển.";
            }
            else
            {
                ViewData["ErrorTitle"] = "Đã có lỗi xảy ra";
                ViewData["ErrorMessage"] = "Hệ thống đang gặp sự cố tạm thời. Vui lòng thử lại sau.";
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
