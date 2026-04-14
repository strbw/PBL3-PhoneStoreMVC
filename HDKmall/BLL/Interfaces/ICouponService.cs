using HDKmall.ViewModels;
using HDKmall.Models;

namespace HDKmall.BLL.Interfaces
{
    public interface ICouponService
    {
        Coupon ValidateCoupon(string code);
        bool ApplyCoupon(int couponId);
        IEnumerable<Coupon> GetAllCoupons();
        void CreateCoupon(Coupon coupon);
        void UpdateCoupon(int id, Coupon coupon);
        void DeleteCoupon(int id);
    }
}
