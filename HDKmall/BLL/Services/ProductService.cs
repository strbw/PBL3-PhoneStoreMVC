using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using HDKmall.ViewModels;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            // Handle main product image
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                var result = await _photoService.AddPhotoAsync(vm.ImageFile);
                if (result.Error == null)
                    product.ImageUrl = result.SecureUrl.AbsoluteUri;
            }

            _productRepository.Add(product);

            if (vm.Versions != null && vm.Versions.Count > 0)
            {
                foreach (var vVM in vm.Versions)
                {
                    if (string.IsNullOrWhiteSpace(vVM.Name)) continue;

                    var version = new ProductVersion
                    {
                        ProductId = product.Id,
                        Name = vVM.Name,
                        BasePrice = vVM.BasePrice,
                        Description = vVM.Description
                    };

                    // Handle version main image
                    if (vVM.ImageFile != null && vVM.ImageFile.Length > 0)
                    {
                        var result = await _photoService.AddPhotoAsync(vVM.ImageFile);
                        if (result.Error == null)
                            version.ImageUrl = result.SecureUrl.AbsoluteUri;
                    }

                    _productRepository.AddVersion(version);

                    // Handle variants (Colors) for this version
                    if (vVM.Variants != null)
                    {
                        foreach (var varVM in vVM.Variants)
                        {
                            if (string.IsNullOrWhiteSpace(varVM.Color)) continue;

                            string? variantImageUrl = null;
                            if (varVM.ImageFile != null && varVM.ImageFile.Length > 0)
                            {
                                var upload = await _photoService.AddPhotoAsync(varVM.ImageFile);
                                if (upload.Error == null)
                                    variantImageUrl = upload.SecureUrl.AbsoluteUri;
                            }

                            var variant = new ProductVariant
                            {
                                ProductVersionId = version.Id,
                                Color = varVM.Color,
                                Price = varVM.Price > 0 ? varVM.Price : version.BasePrice,
                                Stock = varVM.Stock,
                                ImageUrl = variantImageUrl
                            };
                            _productRepository.AddVariant(variant);
                        }
                    }

                    // Handle specifications for this version
                    if (vVM.Specifications != null)
                    {
                        int order = 0;
                        foreach (var sVM in vVM.Specifications)
                        {
                            if (string.IsNullOrWhiteSpace(sVM.SpecName)) continue;
                            var spec = new ProductSpecification
                            {
                                ProductVersionId = version.Id,
                                SpecName = sVM.SpecName,
                                SpecValue = sVM.SpecValue ?? "",
                                DisplayOrder = sVM.DisplayOrder > 0 ? sVM.DisplayOrder : order++
                            };
                            _productRepository.AddSpecification(spec);
                        }
                    }

                    // Handle additional images for this version
                    if (vVM.AdditionalImages != null)
                    {
                        int order = 0;
                        foreach (var file in vVM.AdditionalImages)
                        {
                            if (file == null || file.Length == 0) continue;
                            var result = await _photoService.AddPhotoAsync(file);
                            if (result.Error != null) continue;

                            var img = new ProductImage
                            {
                                ProductVersionId = version.Id,
                                ImageUrl = result.SecureUrl.AbsoluteUri,
                                PublicId = result.PublicId,
                                IsMain = order == 0 && string.IsNullOrEmpty(version.ImageUrl),
                                DisplayOrder = order++
                            };
                            _productRepository.AddImage(img);
                        }
                    }
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

            // Handle main product image update
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                var result = await _photoService.AddPhotoAsync(vm.ImageFile);
                if (result.Error == null)
                    product.ImageUrl = result.SecureUrl.AbsoluteUri;
            }
            else
            {
                product.ImageUrl = vm.ImageUrl; // Keep existing if no new upload
            }

            _productRepository.Update(product);

            // For simplicity in update, we clear and recreate versions (CellPhones logic is complex, this is a safe way for CRUD)
            _productRepository.DeleteVersions(product.Id);

            if (vm.Versions != null && vm.Versions.Count > 0)
            {
                foreach (var vVM in vm.Versions)
                {
                    if (string.IsNullOrWhiteSpace(vVM.Name)) continue;

                    var version = new ProductVersion
                    {
                        ProductId = product.Id,
                        Name = vVM.Name,
                        BasePrice = vVM.BasePrice,
                        Description = vVM.Description,
                        ImageUrl = vVM.ImageUrl
                    };

                    if (vVM.ImageFile != null && vVM.ImageFile.Length > 0)
                    {
                        var result = await _photoService.AddPhotoAsync(vVM.ImageFile);
                        if (result.Error == null)
                            version.ImageUrl = result.SecureUrl.AbsoluteUri;
                    }

                    _productRepository.AddVersion(version);

                    if (vVM.Variants != null)
                    {
                        foreach (var varVM in vVM.Variants)
                        {
                            if (string.IsNullOrWhiteSpace(varVM.Color)) continue;

                            string? variantImageUrl = varVM.ImageUrl;
                            if (varVM.ImageFile != null && varVM.ImageFile.Length > 0)
                            {
                                var upload = await _photoService.AddPhotoAsync(varVM.ImageFile);
                                if (upload.Error == null)
                                    variantImageUrl = upload.SecureUrl.AbsoluteUri;
                            }

                            var variant = new ProductVariant
                            {
                                ProductVersionId = version.Id,
                                Color = varVM.Color,
                                Price = varVM.Price > 0 ? varVM.Price : version.BasePrice,
                                Stock = varVM.Stock,
                                ImageUrl = variantImageUrl
                            };
                            _productRepository.AddVariant(variant);
                        }
                    }

                    if (vVM.Specifications != null)
                    {
                        int order = 0;
                        foreach (var sVM in vVM.Specifications)
                        {
                            if (string.IsNullOrWhiteSpace(sVM.SpecName)) continue;
                            var spec = new ProductSpecification
                            {
                                ProductVersionId = version.Id,
                                SpecName = sVM.SpecName,
                                SpecValue = sVM.SpecValue ?? "",
                                DisplayOrder = sVM.DisplayOrder > 0 ? sVM.DisplayOrder : order++
                            };
                            _productRepository.AddSpecification(spec);
                        }
                    }
                }
            }

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return false;

            // Delete images from cloud (simplified)
            // ... (optional logic for cleanup)

            _productRepository.Delete(id);
            return true;
        }
    }
}