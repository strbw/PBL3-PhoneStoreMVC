using HDKmall.ViewModels;
using HDKmall.Models;

namespace HDKmall.BLL.Interfaces
{
    public interface IOrderService
    {
        Order CreateOrder(int userId, List<CartItem> items, string address, string paymentMethod, decimal totalAmount, decimal discountAmount = 0);
        IEnumerable<Order> GetUserOrders(int userId);
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
        void UpdateOrderStatus(int id, string status);
        void CancelOrder(int id);
    }
}
