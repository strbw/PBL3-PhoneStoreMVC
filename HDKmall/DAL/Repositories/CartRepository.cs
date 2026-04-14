using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using Microsoft.EntityFrameworkCore;

namespace HDKmall.DAL.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ShoppingCart GetCartByUserId(int userId)
        {
            return _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);
        }

        public ShoppingCart GetCartBySessionId(string sessionId)
        {
            return _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.SessionId == sessionId);
        }

        public ShoppingCart GetCartById(int id)
        {
            return _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<ShoppingCart> GetAll()
        {
            return _context.ShoppingCarts.Include(c => c.Items).ToList();
        }

        public void Add(ShoppingCart cart)
        {
            _context.ShoppingCarts.Add(cart);
        }

        public void Update(ShoppingCart cart)
        {
            _context.ShoppingCarts.Update(cart);
        }

        public void Delete(int id)
        {
            var cart = GetCartById(id);
            if (cart != null)
            {
                _context.ShoppingCarts.Remove(cart);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
