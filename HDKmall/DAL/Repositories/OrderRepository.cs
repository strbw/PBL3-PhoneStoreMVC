using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HDKmall.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order GetById(int id)
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                        .ThenInclude(v => v.ProductVersion)
                .FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> GetOrdersByUserId(int userId)
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                        .ThenInclude(v => v.ProductVersion)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                        .ThenInclude(v => v.ProductVersion)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public void Delete(int id)
        {
            var order = GetById(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
