using HDKmall.Models;

namespace HDKmall.DAL.Interfaces
{
    public interface ICartRepository
    {
        ShoppingCart GetCartByUserId(int userId);
        ShoppingCart GetCartBySessionId(string sessionId);
        ShoppingCart GetCartById(int id);
        IEnumerable<ShoppingCart> GetAll();
        void Add(ShoppingCart cart);
        void Update(ShoppingCart cart);
        void Delete(int id);
        void SaveChanges();
    }
}
