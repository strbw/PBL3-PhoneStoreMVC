using HDKmall.Models;
using Microsoft.AspNetCore.Http;

namespace HDKmall.ViewModels
{
    public class AddReviewVM
    {
        public int ProductVersionId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public List<string>? Tags { get; set; } 
        public IFormFile? ImageFile { get; set; }
    }

    public class ReviewListVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool CanDelete { get; set; }
    }
}
