using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HDKmall.ViewModels
{
    public class ProductVersionVM
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên dung lượng")]
        public string Name { get; set; } // e.g. "256GB"

        [Required(ErrorMessage = "Vui lòng nhập giá cơ bản")]
        public decimal BasePrice { get; set; }

        public string? Description { get; set; } // Đặc điểm nổi bật

        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }

        public List<ProductVariantVM> Variants { get; set; } = new List<ProductVariantVM>();
        public List<ProductSpecVM> Specifications { get; set; } = new List<ProductSpecVM>();
        
        // For additional images
        public List<IFormFile>? AdditionalImages { get; set; }
    }
}
