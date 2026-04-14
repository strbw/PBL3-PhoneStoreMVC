using HDKmall.Models;

namespace HDKmall.DAL.Interfaces
{
    public interface IBrandRepository
    {
        IEnumerable<Brand> GetAll();
        Brand GetById(int id);
        void Add(Brand brand);
        void Update(Brand brand);
        void Delete(int id);
        void SaveChanges();
    }
}
