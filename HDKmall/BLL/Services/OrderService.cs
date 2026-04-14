using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;

namespace HDKmall.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
                    Quantity = i.Quantity,
                    UnitPrice = i.Product?.Price ?? 0
                }).ToList()
            };

            _orderRepository.Add(order);
            _orderRepository.SaveChanges();
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
                order.Status = status;
                _orderRepository.Update(order);
                _orderRepository.SaveChanges();
            }
        }

        public void CancelOrder(int id)
        {
            UpdateOrderStatus(id, "Cancelled");
        }
    }
}
