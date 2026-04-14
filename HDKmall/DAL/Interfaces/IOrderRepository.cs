using HDKmall.Models;

namespace HDKmall.DAL.Interfaces
{
    public interface IOrderRepository
    {
        Order GetById(int id);
        IEnumerable<Order> GetOrdersByUserId(int userId);
        IEnumerable<Order> GetAll();
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
        void SaveChanges();
    }
}
