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
        private readonly IPhotoService _photoService; // Thêm dịch vụ xử lý ảnh

        // Cập nhật constructor để nhận thêm IPhotoService
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
                ProductId = vm.ProductId,
                UserId = userId,
                Rating = Math.Min(5, Math.Max(1, vm.Rating)), // Đảm bảo rating từ 1-5
                Comment = vm.Comment,
                CreatedAt = DateTime.Now,
                Status = "Approved" // Tự động duyệt sau khi khách mua hàng xong và gửi đánh giá
            };

            // Ép mảng Tags (List) thành chuỗi JSON để lưu vào Database
            if (vm.Tags != null && vm.Tags.Any())
            {
                review.Tags = JsonSerializer.Serialize(vm.Tags);
            }

            // Gọi Cloudinary upload ảnh nếu người dùng có đính kèm file
            if (vm.ImageFile != null)
            {
                var uploadResult = await _photoService.AddPhotoAsync(vm.ImageFile);
                if (uploadResult.Error == null)
                {
                    review.ImageUrl = uploadResult.SecureUrl.ToString();
                }
            }

            _reviewRepository.Add(review);
            _reviewRepository.SaveChanges(); // Nhớ lưu xuống DB
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
                Tags = r.Tags,           // Cột mới
                ImageUrl = r.ImageUrl,   // Cột mới
                Status = r.Status        // Cột mới
            });
        }

        public double GetAverageRating(int productId)
        {
            var reviews = _reviewRepository.GetApprovedByProductId(productId).ToList();
            if (!reviews.Any()) return 0;
            return reviews.Average(r => r.Rating);
        }

        public void DeleteReview(int id)
        {
            _reviewRepository.Delete(id);
            _reviewRepository.SaveChanges();
        }

        public bool UserCanReview(int userId, int productId)
        {
            // 1. Kiểm tra xem người dùng đã mua sản phẩm này và đơn hàng đã hoàn tất (Delivered) chưa
            var userOrders = _orderRepository.GetOrdersByUserId(userId);
            var hasBought = userOrders.Any(o =>
                o.Status == "Delivered" &&
                o.OrderDetails.Any(od => od.ProductId == productId));

            if (!hasBought) return false;

            // 2. Kiểm tra xem người dùng đã đánh giá sản phẩm này chưa (mỗi người chỉ được đánh giá 1 lần)
            var userReviews = _reviewRepository.GetByUserId(userId);
            var hasReviewed = userReviews.Any(r => r.ProductId == productId);

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