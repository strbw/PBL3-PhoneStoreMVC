using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using HDKmall.ViewModels;

namespace HDKmall.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;

        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
        }

        public Review AddReview(int productId, int userId, int rating, string comment)
        {
            var review = new Review
            {
                ProductId = productId,
                UserId = userId,
                Rating = Math.Min(5, Math.Max(1, rating)),
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _reviewRepository.Add(review);
            _reviewRepository.SaveChanges();
            return review;
        }

        public IEnumerable<ReviewVM> GetProductReviews(int productId)
        {
            var reviews = _reviewRepository.GetByProductId(productId);
            return reviews.Select(r => new ReviewVM
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                UserName = r.User?.FullName ?? "Anonymous",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            });
        }

        public void DeleteReview(int id)
        {
            _reviewRepository.Delete(id);
            _reviewRepository.SaveChanges();
        }

        public double GetAverageRating(int productId)
        {
            var reviews = _reviewRepository.GetByProductId(productId).ToList();
            if (!reviews.Any()) return 0;
            return reviews.Average(r => r.Rating);
        }

        public bool UserCanReview(int userId, int productId)
        {
            // Check if user has purchased this product
            var userOrders = _orderRepository.GetOrdersByUserId(userId);
            return userOrders.Any(o => o.OrderDetails.Any(od => od.ProductId == productId));
        }
    }
}
