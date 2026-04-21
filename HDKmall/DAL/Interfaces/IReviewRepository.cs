using HDKmall.Models;

namespace HDKmall.DAL.Interfaces
{
    public interface IReviewRepository
    {
        Review GetById(int id);
        IEnumerable<Review> GetByProductId(int productId);
        IEnumerable<Review> GetApprovedByProductId(int productId);
        IEnumerable<Review> GetByUserId(int userId);
        IEnumerable<Review> GetAll();
        void Add(Review review);
        void Update(Review review);
        void Delete(int id);
        void SaveChanges();
        IEnumerable<ReviewTag> GetAllTags();
    }
}
