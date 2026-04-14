using HDKmall.Models;

namespace HDKmall.DAL.Interfaces
{
    public interface ICouponRepository
    {
        Coupon GetById(int id);
        Coupon GetByCode(string code);
        IEnumerable<Coupon> GetAll();
        void Add(Coupon coupon);
        void Update(Coupon coupon);
        void Delete(int id);
        void SaveChanges();
    }
}
