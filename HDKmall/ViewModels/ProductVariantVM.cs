using Microsoft.AspNetCore.Http;

namespace HDKmall.ViewModels
{
    public class ProductVariantVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? Color { get; set; }
        public string? Capacity { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }

        public bool InStock => Stock > 0;
    }
}