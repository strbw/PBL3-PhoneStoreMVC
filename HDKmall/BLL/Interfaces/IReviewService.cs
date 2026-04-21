using HDKmall.ViewModels;
using HDKmall.Models;

namespace HDKmall.BLL.Interfaces
{
    public interface IReviewService
    {
        Task<Review> AddReviewAsync(int productId, int userId, int rating, string comment, List<IFormFile> images, List<int> tagIds);
        IEnumerable<ReviewVM> GetProductReviews(int productId);
        IEnumerable<Review> GetAllReviews();
        void DeleteReview(int id);
        void ApproveReview(int id);
        void HideReview(int id);
        void UnhideReview(int id);
        double GetAverageRating(int productId);
        bool UserCanReview(int userId, int productId);
        IEnumerable<ReviewTag> GetAllTags();
    }
}
