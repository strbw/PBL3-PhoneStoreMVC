using HDKmall.Models;

namespace HDKmall.BLL.Interfaces
{
    public interface IBannerService
    {
        IEnumerable<Banner> GetAllBanners();
        Banner GetBannerById(int id);
        void CreateBanner(Banner banner);
        void UpdateBanner(int id, Banner banner);
        void DeleteBanner(int id);
    }
}
