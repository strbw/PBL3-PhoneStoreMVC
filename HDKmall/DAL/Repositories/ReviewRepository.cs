using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using Microsoft.EntityFrameworkCore;

namespace HDKmall.DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Review GetById(int id)
        {
            return _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .Include(r => r.Images)
                .Include(r => r.TagMappings).ThenInclude(tm => tm.Tag)
                .FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Review> GetByProductId(int productId)
        {
            return _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Images)
                .Include(r => r.TagMappings).ThenInclude(tm => tm.Tag)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public IEnumerable<Review> GetApprovedByProductId(int productId)
        {
            return _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Images)
                .Include(r => r.TagMappings).ThenInclude(tm => tm.Tag)
                .Where(r => r.ProductId == productId && r.IsApproved && !r.IsHidden)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public IEnumerable<Review> GetByUserId(int userId)
        {
            return _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.Images)
                .Include(r => r.TagMappings).ThenInclude(tm => tm.Tag)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public IEnumerable<Review> GetAll()
        {
            return _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .Include(r => r.Images)
                .Include(r => r.TagMappings).ThenInclude(tm => tm.Tag)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public void Add(Review review)
        {
            _context.Reviews.Add(review);
        }

        public void Update(Review review)
        {
            _context.Reviews.Update(review);
        }

        public void Delete(int id)
        {
            var review = GetById(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<ReviewTag> GetAllTags()
        {
            return _context.ReviewTags.OrderBy(t => t.DisplayOrder).ToList();
        }
    }
}
