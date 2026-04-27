using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HDKmall.ViewModels
{
    public class ProductVersionVM
    {
        public int Id { get; set; }
        
        public string? Name { get; set; } // e.g. "256GB"

        public decimal BasePrice { get; set; }

        public decimal? OriginalPrice { get; set; }
        public int DiscountPercent { get; set; }

        public string? Description { get; set; } // Đặc điểm nổi bật

        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }

        public List<ProductVariantVM> Variants { get; set; } = new List<ProductVariantVM>();
        public List<ProductSpecVM> Specifications { get; set; } = new List<ProductSpecVM>();
        
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }

        // For additional images
        public List<IFormFile>? AdditionalImages { get; set; }
    }
}
