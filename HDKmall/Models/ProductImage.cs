using System.ComponentModel.DataAnnotations;

namespace HDKmall.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ImageUrl { get; set; }
        public string? PublicId { get; set; }
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }
}
