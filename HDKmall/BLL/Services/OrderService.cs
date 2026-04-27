using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDKmall.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public Order CreateOrder(int userId, List<CartItem> items, string address, string paymentMethod, decimal totalAmount, decimal discountAmount = 0)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Address = address,
                PaymentMethod = paymentMethod,
                TotalAmount = totalAmount - discountAmount,
                Status = "Pending",
                OrderDetails = items.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    ProductVariantId = i.VariantId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Variant != null ? i.Variant.Price : (i.Product?.Price ?? 0)
                }).ToList()
            };

            _orderRepository.Add(order);
            _orderRepository.SaveChanges();
            _logger.LogInformation("Đã tạo đơn hàng mới #{OrderId} cho người dùng {UserId}. Tổng tiền: {TotalAmount}", order.Id, userId, order.TotalAmount);
            return order;
        }

        public IEnumerable<Order> GetUserOrders(int userId)
        {
            return _orderRepository.GetOrdersByUserId(userId);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAll();
        }

        public Order GetOrderById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public void UpdateOrderStatus(int id, string status)
        {
            var order = _orderRepository.GetById(id);
            if (order != null)
            {
                var oldStatus = order.Status;
                order.Status = status;
                _orderRepository.Update(order);
                _orderRepository.SaveChanges();
                _logger.LogInformation("Đơn hàng #{OrderId} đã chuyển trạng thái từ '{OldStatus}' sang '{NewStatus}'", id, oldStatus, status);
            }
        }

        public void CancelOrder(int id)
        {
            UpdateOrderStatus(id, "Cancelled");
        }

        public void DeleteOrder(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order != null)
            {
                _orderRepository.Delete(id);
                _orderRepository.SaveChanges();
                _logger.LogInformation("Đã xóa hoàn toàn đơn hàng #{OrderId} do thanh toán thất bại", id);
            }
        }
    }
}
