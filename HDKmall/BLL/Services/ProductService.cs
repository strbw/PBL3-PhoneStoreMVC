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
                CategoryId = vm.CategoryId,
                BrandId = vm.BrandId
            };

            // Handle single main image (backward compat)
            if (vm.Image != null && vm.Image.Length > 0)
            {
                var result = await _photoService.AddPhotoAsync(vm.Image);
                if (result.Error != null) return false;
                product.ImageUrl = result.SecureUrl.AbsoluteUri;
                product.PublicId = result.PublicId;
            }

            _productRepository.Add(product);

            // Handle multiple images
            if (vm.Images != null && vm.Images.Count > 0)
            {
                bool isFirst = string.IsNullOrEmpty(product.ImageUrl);
                int order = 0;
                foreach (var file in vm.Images)
                {
                    if (file == null || file.Length == 0) continue;
                    var result = await _photoService.AddPhotoAsync(file);
                    if (result.Error != null) continue;

                    var img = new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = result.SecureUrl.AbsoluteUri,
                        PublicId = result.PublicId,
                        IsMain = isFirst && order == 0,
                        DisplayOrder = order++
                    };
                    _productRepository.AddImage(img);

                    if (isFirst && img.IsMain)
                    {
                        product.ImageUrl = img.ImageUrl;
                        product.PublicId = img.PublicId;
                        _productRepository.Update(product);
                    }
                }
            }

            // Handle variants
            if (vm.Variants != null && vm.Variants.Count > 0)
            {
                foreach (var v in vm.Variants)
                {
                    if (string.IsNullOrWhiteSpace(v.Color) && string.IsNullOrWhiteSpace(v.Capacity)) continue;

                    string? variantImageUrl = v.ImageUrl;

                    if (v.ImageFile != null && v.ImageFile.Length > 0)
                    {
                        var upload = await _photoService.AddPhotoAsync(v.ImageFile);
                        if (upload.Error == null)
                            variantImageUrl = upload.SecureUrl.AbsoluteUri;
                    }

                    var variant = new ProductVariant
                    {
                        ProductId = product.Id,
                        Color = v.Color,
                        Capacity = v.Capacity,
                        Price = v.Price,
                        Stock = v.Stock,
                        ImageUrl = variantImageUrl
                    };
                    _productRepository.AddVariant(variant);
                }
            }

            // Handle specifications
            if (vm.Specifications != null && vm.Specifications.Count > 0)
            {
                int order = 0;
                foreach (var s in vm.Specifications)
                {
                    if (string.IsNullOrWhiteSpace(s.SpecName)) continue;
                    var spec = new ProductSpecification
                    {
                        ProductId = product.Id,
                        SpecName = s.SpecName,
                        SpecValue = s.SpecValue ?? "",
                        DisplayOrder = s.DisplayOrder > 0 ? s.DisplayOrder : order++
                    };
                    _productRepository.AddSpecification(spec);
                }
            }

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
            product.BrandId = vm.BrandId;

            // Handle single main image replacement
            if (vm.Image != null && vm.Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(product.PublicId))
                    await _photoService.DeletePhotoAsync(product.PublicId);

                var result = await _photoService.AddPhotoAsync(vm.Image);
                if (result.Error != null) return false;
                product.ImageUrl = result.SecureUrl.AbsoluteUri;
                product.PublicId = result.PublicId;
            }

            _productRepository.Update(product);

            // Handle new additional images
            if (vm.Images != null && vm.Images.Count > 0)
            {
                var existingImages = product.Images?.ToList() ?? new List<ProductImage>();
                int order = existingImages.Count > 0 ? existingImages.Max(i => i.DisplayOrder) + 1 : 0;
                bool hasMain = existingImages.Any(i => i.IsMain);

                foreach (var file in vm.Images)
                {
                    if (file == null || file.Length == 0) continue;
                    var result = await _photoService.AddPhotoAsync(file);
                    if (result.Error != null) continue;

                    var img = new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = result.SecureUrl.AbsoluteUri,
                        PublicId = result.PublicId,
                        IsMain = !hasMain && order == 0,
                        DisplayOrder = order++
                    };
                    _productRepository.AddImage(img);
                    hasMain = true;
                }
            }

            // Replace variants (delete old, add new)
            if (vm.Variants != null)
            {
                _productRepository.DeleteVariants(product.Id);
                foreach (var v in vm.Variants)
                {
                    if (string.IsNullOrWhiteSpace(v.Color) && string.IsNullOrWhiteSpace(v.Capacity)) continue;

                    string? variantImageUrl = v.ImageUrl;

                    if (v.ImageFile != null && v.ImageFile.Length > 0)
                    {
                        var upload = await _photoService.AddPhotoAsync(v.ImageFile);
                        if (upload.Error == null)
                            variantImageUrl = upload.SecureUrl.AbsoluteUri;
                    }

                    var variant = new ProductVariant
                    {
                        ProductId = product.Id,
                        Color = v.Color,
                        Capacity = v.Capacity,
                        Price = v.Price,
                        Stock = v.Stock,
                        ImageUrl = variantImageUrl
                    };
                    _productRepository.AddVariant(variant);
                }
            }

            // Replace specifications (delete old, add new)
            if (vm.Specifications != null)
            {
                _productRepository.DeleteSpecifications(product.Id);
                int order = 0;
                foreach (var s in vm.Specifications)
                {
                    if (string.IsNullOrWhiteSpace(s.SpecName)) continue;
                    var spec = new ProductSpecification
                    {
                        ProductId = product.Id,
                        SpecName = s.SpecName,
                        SpecValue = s.SpecValue ?? "",
                        DisplayOrder = s.DisplayOrder > 0 ? s.DisplayOrder : order++
                    };
                    _productRepository.AddSpecification(spec);
                }
            }

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return false;

            if (!string.IsNullOrEmpty(product.PublicId))
                await _photoService.DeletePhotoAsync(product.PublicId);

            if (product.Images != null)
            {
                foreach (var img in product.Images)
                {
                    if (!string.IsNullOrEmpty(img.PublicId))
                        await _photoService.DeletePhotoAsync(img.PublicId);
                }
            }

            _productRepository.Delete(id);
            return true;
        }
    }
}