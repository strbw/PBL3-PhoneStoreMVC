using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
                .Include(c => c.Items)
                    .ThenInclude(i => i.Variant)
                        .ThenInclude(v => v.ProductVersion)
                .FirstOrDefault(c => c.UserId == userId);
        }

        public ShoppingCart GetCartBySessionId(string sessionId)
        {
            return _context.ShoppingCarts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .Include(c => c.Items)
                    .ThenInclude(i => i.Variant)
                        .ThenInclude(v => v.ProductVersion)
                .FirstOrDefault(c => c.SessionId == sessionId);
        }

        public ShoppingCart GetCartById(int id)
        {
            return _context.ShoppingCarts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .Include(c => c.Items)
                    .ThenInclude(i => i.Variant)
                        .ThenInclude(v => v.ProductVersion)
                .FirstOrDefault(c => c.Id == id);
        }

        public CartItem GetCartItemById(int cartItemId)
        {
            return _context.CartItems
                .Include(i => i.Product)
                .Include(i => i.Variant)
                    .ThenInclude(v => v.ProductVersion)
                .FirstOrDefault(i => i.Id == cartItemId);
        }

        public void UpdateCartItem(CartItem item)
        {
            _context.CartItems.Update(item);
        }

        public void RemoveCartItem(CartItem item)
        {
            _context.CartItems.Remove(item);
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
