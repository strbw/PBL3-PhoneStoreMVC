using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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

        public ReviewService(
            IReviewRepository reviewRepository, 
            IOrderRepository orderRepository,
            IPhotoService photoService)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
            _photoService = photoService;
        }

        public async Task<Review> AddReviewAsync(AddReviewVM vm, int userId)
        {
            var review = new Review
            {
                ProductVersionId = vm.ProductVersionId,
                UserId = userId,
                Rating = Math.Min(5, Math.Max(1, vm.Rating)),
                Comment = vm.Comment,
                CreatedAt = DateTime.Now,
                Status = "Approved"
            };

            if (vm.Tags != null && vm.Tags.Any())
            {
                review.Tags = JsonSerializer.Serialize(vm.Tags);
            }

            if (vm.ImageFile != null)
            {
                var uploadResult = await _photoService.AddPhotoAsync(vm.ImageFile);
                if (uploadResult.Error == null)
                {
                    review.ImageUrl = uploadResult.SecureUrl.ToString();
                }
            }

            _reviewRepository.Add(review);
            _reviewRepository.SaveChanges();
            return review;
        }

        public IEnumerable<ReviewVM> GetProductReviews(int productVersionId)
        {
            var reviews = _reviewRepository.GetApprovedByVersionId(productVersionId);
            return reviews.Select(r => new ReviewVM
            {
                Id = r.Id,
                ProductVersionId = r.ProductVersionId,
                UserId = r.UserId,
                UserName = r.User?.FullName ?? "Anonymous",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                Tags = r.Tags,
                ImageUrl = r.ImageUrl,
                Status = r.Status
            });
        }

        public double GetAverageRating(int productVersionId)
        {
            var reviews = _reviewRepository.GetApprovedByVersionId(productVersionId).ToList();
            if (!reviews.Any()) return 0;
            return reviews.Average(r => r.Rating);
        }

        public void DeleteReview(int id)
        {
            _reviewRepository.Delete(id);
            _reviewRepository.SaveChanges();
        }

        public bool UserCanReview(int userId, int productVersionId)
        {
            var userOrders = _orderRepository.GetOrdersByUserId(userId);
            // Check if user bought any product that has this version
            // Actually, usually order details link to variant or product.
            // Need to check OrderDetail model.
            var hasBought = userOrders.Any(o =>
                o.Status == "Delivered" &&
                o.OrderDetails.Any(od => od.ProductVariant != null && od.ProductVariant.ProductVersionId == productVersionId));

            if (!hasBought) return false;

            var userReviews = _reviewRepository.GetByUserId(userId);
            var hasReviewed = userReviews.Any(r => r.ProductVersionId == productVersionId);

            return !hasReviewed;
        }

        public IEnumerable<Review> GetAllReviews()
        {
            return _reviewRepository.GetAll();
        }

        public void ApproveReview(int id)
        {
            _reviewRepository.UpdateStatus(id, "Approved");
        }

        public void HideReview(int id)
        {
            _reviewRepository.UpdateStatus(id, "Hidden");
        }
    }
}