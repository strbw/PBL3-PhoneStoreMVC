using HDKmall.Models;

namespace HDKmall.BLL.Interfaces
{
    public interface IBrandService
    {
        IEnumerable<Brand> GetAllBrands();
        Brand GetBrandById(int id);
        Task<bool> AddBrandAsync(Brand brand);
        Task<bool> UpdateBrandAsync(int id, Brand brand);
        Task<bool> DeleteBrandAsync(int id);
    }
}
