using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using HDKmall.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HDKmall.BLL.Services
{
    public class ProductSearchService : IProductSearchService
    {
        private readonly IProductRepository _productRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<ProductSearchService> _logger;

        public ProductSearchService(IProductRepository productRepository, IReviewRepository reviewRepository, ILogger<ProductSearchService> logger)
        {
            _productRepository = productRepository;
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        public PaginationVM SearchProducts(ProductFilterVM filter)
        {
            var products = _productRepository.GetAll();
            var query = products.AsQueryable();

            // Apply filters
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
            }

            if (filter.BrandId.HasValue)
            {
                query = query.Where(p => p.BrandId == filter.BrandId.Value);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(filter.SearchQuery))
            {
                var searchTerm = filter.SearchQuery.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm) || 
                                         p.Description.ToLower().Contains(searchTerm));
            }

            // Apply sorting
            switch (filter.SortBy)
            {
                case "price-low":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price-high":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case "rating":
                    // Would need to join with reviews
                    query = query.OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0);
                    break;
                default: // newest
                    query = query.OrderByDescending(p => p.Id);
                    break;
            }

            var totalCount = query.Count();
            var products_page = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            var productVMs = products_page.Select(p => new ProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                BrandId = p.BrandId,
                BrandName = p.Brand?.Name,
                AverageRating = p.Reviews.Any(r => r.Status == "Approved") ? p.Reviews.Where(r => r.Status == "Approved").Average(r => r.Rating) : 0,
                ReviewCount = p.Reviews.Count(r => r.Status == "Approved")
            }).ToList();

            return new PaginationVM
            {
                Products = productVMs,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public List<ProductListVM> GetProductsByCategory(int categoryId)
        {
            var products = _productRepository.GetAll()
                .Where(p => p.CategoryId == categoryId)
                .ToList();

            return MapToProductListVM(products);
        }

        public List<ProductListVM> GetProductsByBrand(int brandId)
        {
            var products = _productRepository.GetAll()
                .Where(p => p.BrandId == brandId)
                .ToList();

            return MapToProductListVM(products);
        }

        public List<ProductListVM> GetFeaturedProducts(int take = 10)
        {
            var products = _productRepository.GetAll()
                .OrderByDescending(p => p.Reviews.Count)
                .Take(take)
                .ToList();

            return MapToProductListVM(products);
        }

        public List<ProductListVM> GetNewProducts(int take = 10)
        {
            var products = _productRepository.GetAll()
                .OrderByDescending(p => p.Id)
                .Take(take)
                .ToList();

            return MapToProductListVM(products);
        }

        public ProductDetailVM GetProductDetail(int id)
        {
            var product = _productRepository.GetById(id);
            return MapToProductDetailVM(product);
        }

        public ProductDetailVM GetProductDetailBySlug(string slug)
        {
            var product = _productRepository.GetBySlug(slug);
            return MapToProductDetailVM(product);
        }

        private ProductDetailVM MapToProductDetailVM(Product product)
        {
            if (product == null) return null;

            return new ProductDetailVM
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                BasePrice = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                Category = product.Category,
                BrandId = product.BrandId,
                Brand = product.Brand,
                Variants = product.Variants.Select(v => new ProductVariantVM
                {
                    Id = v.Id,
                    ProductId = v.ProductId,
                    Color = v.Color,
                    Capacity = v.Capacity,
                    Price = v.Price,
                    Stock = v.Stock,
                    ImageUrl = v.ImageUrl
                }).ToList(),
                Reviews = product.Reviews
                    .Where(r => r.Status == "Approved")
                    .Select(r => new ReviewVM
                    {
                        Id = r.Id,
                        ProductId = r.ProductId,
                        UserId = r.UserId,
                        UserName = r.User?.FullName ?? "Anonymous",
                        Rating = r.Rating,
                        Comment = r.Comment,
                        CreatedAt = r.CreatedAt,
                        Tags = r.Tags,
                        ImageUrl = r.ImageUrl
                    }).ToList(),
                AverageRating = product.Reviews.Any(r => r.Status == "Approved") 
                    ? product.Reviews.Where(r => r.Status == "Approved").Average(r => r.Rating) 
                    : 0,
                TotalReviews = product.Reviews.Count(r => r.Status == "Approved"),
                Images = (product.Images ?? new List<ProductImage>())
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new ProductImageVM
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                        IsMain = i.IsMain,
                        DisplayOrder = i.DisplayOrder
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
        }

        private List<ProductListVM> MapToProductListVM(List<Product> products)
        {
            return products.Select(p => new ProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                BrandId = p.BrandId,
                BrandName = p.Brand?.Name,
                AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                ReviewCount = p.Reviews.Count
            }).ToList();
        }
    }
}
