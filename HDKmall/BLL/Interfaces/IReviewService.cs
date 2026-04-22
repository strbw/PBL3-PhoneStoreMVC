using System.Threading.Tasks;
using HDKmall.Models;
using HDKmall.ViewModels;

namespace HDKmall.BLL.Interfaces
{
    public interface IReviewService
    {
        
        IEnumerable<ReviewVM> GetProductReviews(int productId);
        IEnumerable<Review> GetAllReviews();
        void DeleteReview(int id);
        double GetAverageRating(int productId);
        bool UserCanReview(int userId, int productId);
        Task<Review> AddReviewAsync(AddReviewVM vm, int userId);
        void ApproveReview(int id);
        void HideReview(int id);
    }
}
