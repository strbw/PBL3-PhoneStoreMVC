using System.ComponentModel.DataAnnotations;

namespace HDKmall.Models
{
    public class ProductSpecification
    {
        [Key]
        public int Id { get; set; }
        public int ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }

        public string SpecName { get; set; }
        public string SpecValue { get; set; }
        public int DisplayOrder { get; set; }
    }
}
