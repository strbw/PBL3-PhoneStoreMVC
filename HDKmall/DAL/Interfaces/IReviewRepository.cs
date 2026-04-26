using HDKmall.Models;
using System.Collections.Generic;

namespace HDKmall.DAL.Interfaces
{
    public interface IReviewRepository
    {
        Review GetById(int id);
        IEnumerable<Review> GetByVersionId(int versionId);
        IEnumerable<Review> GetByUserId(int userId);
        IEnumerable<Review> GetAll();
        IEnumerable<Review> GetByProductId(int productId);
        Review? GetUserReviewForVersion(int userId, int versionId);
        void Add(Review review);
        void Update(Review review);
        void Delete(int id);
        void SaveChanges();
        IEnumerable<Review> GetApprovedByVersionId(int versionId);
        IEnumerable<Review> GetApprovedByProductId(int productId);
        void UpdateStatus(int id, string status);
    }
}
