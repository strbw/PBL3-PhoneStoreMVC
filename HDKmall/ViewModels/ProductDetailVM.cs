using HDKmall.Models;

namespace HDKmall.ViewModels
{
    public class ProductDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<ProductVersionVM> Versions { get; set; } = new List<ProductVersionVM>();
        public List<ProductVariantVM> Variants { get; set; } = new List<ProductVariantVM>();
        public List<ReviewVM> Reviews { get; set; } = new List<ReviewVM>();
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<ProductImageVM> Images { get; set; } = new List<ProductImageVM>();
        public List<ProductSpecVM> Specifications { get; set; } = new List<ProductSpecVM>();
    }
}
