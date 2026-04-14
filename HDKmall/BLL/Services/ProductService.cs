using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using HDKmall.ViewModels;

namespace HDKmall.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPhotoService _photoService;

        public ProductService(IProductRepository productRepository, IPhotoService photoService)
        {
            _productRepository = productRepository;
            _photoService = photoService;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetById(id);
        }

        public async Task<bool> AddProductAsync(ProductVM vm)
        {
            var product = new Product
            {
                Name = vm.Name,
                Price = vm.Price,
                Description = vm.Description,
                CategoryId = vm.CategoryId
            };

            if (vm.Image != null && vm.Image.Length > 0)
            {
                var result = await _photoService.AddPhotoAsync(vm.Image);
                if (result.Error != null) return false;

                product.ImageUrl = result.SecureUrl.AbsoluteUri;
                product.PublicId = result.PublicId;
            }

            _productRepository.Add(product);
            return true;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductVM vm)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return false;

            product.Name = vm.Name;
            product.Price = vm.Price;
            product.Description = vm.Description;
            product.CategoryId = vm.CategoryId;

            if (vm.Image != null && vm.Image.Length > 0)
            {
                // Delete old photo if it exists
                if (!string.IsNullOrEmpty(product.PublicId))
                {
                    await _photoService.DeletePhotoAsync(product.PublicId);
                }

                // Add new photo
                var result = await _photoService.AddPhotoAsync(vm.Image);
                if (result.Error != null) return false;

                product.ImageUrl = result.SecureUrl.AbsoluteUri;
                product.PublicId = result.PublicId;
            }

            _productRepository.Update(product);
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return false;

            if (!string.IsNullOrEmpty(product.PublicId))
            {
                await _photoService.DeletePhotoAsync(product.PublicId);
            }

            _productRepository.Delete(id);
            return true;
        }
    }
}