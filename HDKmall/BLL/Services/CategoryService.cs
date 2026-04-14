using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;

namespace HDKmall.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetCategoryById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            try
            {
                _categoryRepository.Add(category);
                _categoryRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            try
            {
                var existing = _categoryRepository.GetById(id);
                if (existing == null) return false;

                existing.Name = category.Name;
                existing.Description = category.Description;

                _categoryRepository.Update(existing);
                _categoryRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                _categoryRepository.Delete(id);
                _categoryRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
