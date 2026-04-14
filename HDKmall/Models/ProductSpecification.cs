using System.ComponentModel.DataAnnotations;

namespace HDKmall.Models
{
    public class ProductSpecification
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string SpecName { get; set; }
        public string SpecValue { get; set; }
        public int DisplayOrder { get; set; }
    }
}
