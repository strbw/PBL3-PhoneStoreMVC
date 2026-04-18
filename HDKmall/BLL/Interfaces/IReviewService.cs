using HDKmall.ViewModels;
using HDKmall.Models;

namespace HDKmall.BLL.Interfaces
{
    public interface IReviewService
    {
        Review AddReview(int productId, int userId, int rating, string comment);
        IEnumerable<ReviewVM> GetProductReviews(int productId);
        IEnumerable<Review> GetAllReviews();
        void DeleteReview(int id);
        double GetAverageRating(int productId);
        bool UserCanReview(int userId, int productId);
    }
}
