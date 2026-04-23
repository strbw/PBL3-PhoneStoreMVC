using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HDKmall.Models
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }
        public int ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        
        public string? Color { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
    }
}
