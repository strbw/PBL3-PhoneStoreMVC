using System.Collections.Generic;
using System.Threading.Tasks;
using HDKmall.Models;
using HDKmall.ViewModels;

namespace HDKmall.BLL.Interfaces
{
    public interface IReviewService
    {
        IEnumerable<ReviewVM> GetProductReviews(int productVersionId);
        IEnumerable<Review> GetAllReviews();
        void DeleteReview(int id);
        double GetAverageRating(int productVersionId);
        bool UserCanReview(int userId, int productVersionId);
        Task<Review> AddReviewAsync(AddReviewVM vm, int userId);
        void ApproveReview(int id);
        void HideReview(int id);
    }
}
