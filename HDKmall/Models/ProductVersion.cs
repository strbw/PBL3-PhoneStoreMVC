using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HDKmall.Models
{
    public class ProductVersion
    {
        [Key]
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public string Name { get; set; } // e.g. "256GB", "512GB", "1TB"

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }

        public string? Description { get; set; } // "Đặc điểm nổi bật"

        public string? ImageUrl { get; set; } // Default image for this version

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
