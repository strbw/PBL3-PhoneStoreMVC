using System.ComponentModel.DataAnnotations;

namespace HDKmall.Models
{
    public class ReviewTagMapping
    {
        [Key]
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }
        public int ReviewTagId { get; set; }
        public ReviewTag Tag { get; set; }
    }
}
