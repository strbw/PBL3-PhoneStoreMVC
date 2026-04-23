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

        // Existing image URL (for display on edit)
        public string? ImageUrl { get; set; }

        // Main product image file for upload
        public IFormFile? ImageFile { get; set; }

        // Versions (Capacities)
        public List<ProductVersionVM> Versions { get; set; } = new List<ProductVersionVM>();
    }
}
