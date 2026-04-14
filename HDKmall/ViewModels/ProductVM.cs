using System.ComponentModel.DataAnnotations;

namespace HDKmall.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public int? BrandId { get; set; }

        // Single main image (kept for backward compatibility)
        public IFormFile? Image { get; set; }

        // Multiple images upload
        public List<IFormFile>? Images { get; set; }

        // Existing image URL (for display on edit)
        public string? ImageUrl { get; set; }

        // Variants
        public List<ProductVariantVM> Variants { get; set; } = new List<ProductVariantVM>();

        // Specifications
        public List<ProductSpecVM> Specifications { get; set; } = new List<ProductSpecVM>();
    }
}
