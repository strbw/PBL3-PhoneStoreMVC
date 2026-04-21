using HDKmall.Models;

namespace HDKmall.ViewModels
{
    public class AddReviewVM
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public List<int> TagIds { get; set; } = new List<int>();
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
