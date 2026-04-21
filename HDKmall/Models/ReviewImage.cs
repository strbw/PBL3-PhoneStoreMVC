using System.ComponentModel.DataAnnotations;

namespace HDKmall.Models
{
    public class ReviewImage
    {
        [Key]
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }
}
