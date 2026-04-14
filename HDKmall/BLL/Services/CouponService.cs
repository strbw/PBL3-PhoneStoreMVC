using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;

namespace HDKmall.BLL.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public Coupon ValidateCoupon(string code)
        {
            var coupon = _couponRepository.GetByCode(code);
            if (coupon == null) return null;

            // Check if expired
            if (coupon.ExpiryDate < DateTime.Now) return null;

            // Check if usage limit exceeded
            if (coupon.UsedCount >= coupon.UsageLimit) return null;

            return coupon;
        }

        public bool ApplyCoupon(int couponId)
        {
            var coupon = _couponRepository.GetById(couponId);
            if (coupon == null) return false;

            if (coupon.UsedCount >= coupon.UsageLimit) return false;
            if (coupon.ExpiryDate < DateTime.Now) return false;

            coupon.UsedCount++;
            _couponRepository.Update(coupon);
            _couponRepository.SaveChanges();
            return true;
        }

        public IEnumerable<Coupon> GetAllCoupons()
        {
            return _couponRepository.GetAll();
        }

        public void CreateCoupon(Coupon coupon)
        {
            _couponRepository.Add(coupon);
            _couponRepository.SaveChanges();
        }

        public void UpdateCoupon(int id, Coupon coupon)
        {
            var existing = _couponRepository.GetById(id);
            if (existing != null)
            {
                existing.Code = coupon.Code;
                existing.DiscountAmount = coupon.DiscountAmount;
                existing.ExpiryDate = coupon.ExpiryDate;
                existing.UsageLimit = coupon.UsageLimit;
                _couponRepository.Update(existing);
                _couponRepository.SaveChanges();
            }
        }

        public void DeleteCoupon(int id)
        {
            _couponRepository.Delete(id);
            _couponRepository.SaveChanges();
        }
    }
}
