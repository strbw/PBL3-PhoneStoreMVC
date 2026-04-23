using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using System.Linq;
using System.Collections.Generic;

namespace HDKmall.DAL.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;

        public CouponRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Coupon GetById(int id)
        {
            return _context.Coupons.FirstOrDefault(c => c.Id == id);
        }

        public Coupon GetByCode(string code)
        {
            return _context.Coupons.FirstOrDefault(c => c.Code == code);
        }

        public IEnumerable<Coupon> GetAll()
        {
            return _context.Coupons.ToList();
        }

        public void Add(Coupon coupon)
        {
            _context.Coupons.Add(coupon);
        }

        public void Update(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
        }

        public void Delete(int id)
        {
            var coupon = GetById(id);
            if (coupon != null)
            {
                _context.Coupons.Remove(coupon);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
