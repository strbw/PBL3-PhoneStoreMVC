using HDKmall.Models;

namespace HDKmall.DAL.Interfaces
{
    public interface IBannerRepository
    {
        IEnumerable<Banner> GetAll();
        Banner GetById(int id);
        void Add(Banner banner);
        void Update(Banner banner);
        void Delete(int id);
        void SaveChanges();
    }
}
