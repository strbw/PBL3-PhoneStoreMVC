using System.ComponentModel.DataAnnotations;

namespace HDKmall.Models
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string? Title { get; set; }
        public string? LinkUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
