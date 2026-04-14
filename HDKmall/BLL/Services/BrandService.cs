using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;

namespace HDKmall.BLL.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            return _brandRepository.GetAll();
        }

        public Brand GetBrandById(int id)
        {
            return _brandRepository.GetById(id);
        }

        public async Task<bool> AddBrandAsync(Brand brand)
        {
            try
            {
                _brandRepository.Add(brand);
                _brandRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBrandAsync(int id, Brand brand)
        {
            try
            {
                var existing = _brandRepository.GetById(id);
                if (existing == null) return false;

                existing.Name = brand.Name;
                existing.Description = brand.Description;

                _brandRepository.Update(existing);
                _brandRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
            try
            {
                _brandRepository.Delete(id);
                _brandRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
