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
        private readonly IPhotoService _photoService;

        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository, IPhotoService photoService)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
            _photoService = photoService;
        }

        public async Task<Review> AddReviewAsync(int productId, int userId, int rating, string comment, List<IFormFile> images, List<int> tagIds)
        {
            var review = new Review
            {
                ProductId = productId,
                UserId = userId,
                Rating = Math.Min(5, Math.Max(1, rating)),
                Comment = comment,
                CreatedAt = DateTime.Now,
                IsApproved = false,
                IsHidden = false
            };

            _reviewRepository.Add(review);
            _reviewRepository.SaveChanges();

            // Upload images
            if (images != null)
            {
                int order = 0;
                foreach (var img in images)
                {
                    if (img != null && img.Length > 0)
                    {
                        var upload = await _photoService.AddPhotoAsync(img);
                        if (upload.Error == null)
                        {
                            review.Images.Add(new ReviewImage
                            {
                                ReviewId = review.Id,
                                ImageUrl = upload.SecureUrl.AbsoluteUri,
                                DisplayOrder = order++
                            });
                        }
                    }
                }
            }

            // Add tag mappings
            if (tagIds != null)
            {
                foreach (var tagId in tagIds)
                {
                    review.TagMappings.Add(new ReviewTagMapping
                    {
                        ReviewId = review.Id,
                        ReviewTagId = tagId
                    });
                }
            }

            _reviewRepository.Update(review);
            _reviewRepository.SaveChanges();

            return review;
        }

        public IEnumerable<ReviewVM> GetProductReviews(int productId)
        {
            var reviews = _reviewRepository.GetApprovedByProductId(productId);
            return reviews.Select(r => new ReviewVM
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                UserName = r.User?.FullName ?? "Anonymous",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                IsApproved = r.IsApproved,
                IsHidden = r.IsHidden,
                Images = r.Images.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList(),
                Tags = r.TagMappings.Select(tm => (tm.Tag?.Emoji ?? "") + " " + (tm.Tag?.Name ?? "")).ToList()
            });
        }

        public void DeleteReview(int id)
        {
            _reviewRepository.Delete(id);
            _reviewRepository.SaveChanges();
        }

        public void ApproveReview(int id)
        {
            var review = _reviewRepository.GetById(id);
            if (review != null)
            {
                review.IsApproved = true;
                review.IsHidden = false;
                _reviewRepository.Update(review);
                _reviewRepository.SaveChanges();
            }
        }

        public void HideReview(int id)
        {
            var review = _reviewRepository.GetById(id);
            if (review != null)
            {
                review.IsHidden = true;
                _reviewRepository.Update(review);
                _reviewRepository.SaveChanges();
            }
        }

        public void UnhideReview(int id)
        {
            var review = _reviewRepository.GetById(id);
            if (review != null)
            {
                review.IsHidden = false;
                _reviewRepository.Update(review);
                _reviewRepository.SaveChanges();
            }
        }

        public double GetAverageRating(int productId)
        {
            var reviews = _reviewRepository.GetApprovedByProductId(productId).ToList();
            if (!reviews.Any()) return 0;
            return reviews.Average(r => r.Rating);
        }

        public bool UserCanReview(int userId, int productId)
        {
            var userOrders = _orderRepository.GetOrdersByUserId(userId);
            return userOrders.Any(o =>
                o.Status == "Paid" &&
                o.OrderDetails.Any(od => od.ProductId == productId));
        }

        public IEnumerable<Review> GetAllReviews()
        {
            return _reviewRepository.GetAll();
        }

        public IEnumerable<ReviewTag> GetAllTags()
        {
            return _reviewRepository.GetAllTags();
        }
    }
}
