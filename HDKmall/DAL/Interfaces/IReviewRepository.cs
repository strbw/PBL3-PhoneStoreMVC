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
        void Add(Review review);
        void Update(Review review);
        void Delete(int id);
        void SaveChanges();
        IEnumerable<Review> GetApprovedByVersionId(int versionId);
        void UpdateStatus(int id, string status);
    }
}
