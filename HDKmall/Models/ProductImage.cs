using System.ComponentModel.DataAnnotations;

namespace HDKmall.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public int ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }

        public string ImageUrl { get; set; }
        public string? PublicId { get; set; }
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }
}
