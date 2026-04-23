using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace HDKmall.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Versions)
                .ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Versions)
                    .ThenInclude(v => v.Variants)
                .Include(p => p.Versions)
                    .ThenInclude(v => v.Specifications)
                .Include(p => p.Versions)
                    .ThenInclude(v => v.Images)
                .FirstOrDefault(p => p.Id == id);
        }

        public Product GetBySlug(string slug)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Versions)
                    .ThenInclude(v => v.Variants)
                .Include(p => p.Versions)
                    .ThenInclude(v => v.Specifications)
                .Include(p => p.Versions)
                    .ThenInclude(v => v.Images)
                .FirstOrDefault(p => p.Slug == slug);
        }

        public void Add(Product product)
        {
            if (string.IsNullOrEmpty(product.Slug))
            {
                product.Slug = HDKmall.Helpers.SlugHelper.GenerateSlug(product.Name);
            }
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            if (string.IsNullOrEmpty(product.Slug))
            {
                product.Slug = HDKmall.Helpers.SlugHelper.GenerateSlug(product.Name);
            }
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products
                .Include(p => p.Versions)
                    .ThenInclude(v => v.Images)
                .Include(p => p.Versions)
                    .ThenInclude(v => v.Variants)
                .Include(p => p.Versions)
                    .ThenInclude(v => v.Specifications)
                .FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        // Version methods
        public void AddVersion(ProductVersion version)
        {
            _context.ProductVersions.Add(version);
            _context.SaveChanges();
        }

        public void DeleteVersions(int productId)
        {
            var versions = _context.ProductVersions.Where(v => v.ProductId == productId).ToList();
            _context.ProductVersions.RemoveRange(versions);
            _context.SaveChanges();
        }

        // Image methods
        public void AddImage(ProductImage image)
        {
            _context.ProductImages.Add(image);
            _context.SaveChanges();
        }

        public void DeleteImage(int imageId)
        {
            var image = _context.ProductImages.Find(imageId);
            if (image != null)
            {
                _context.ProductImages.Remove(image);
                _context.SaveChanges();
            }
        }

        // Variant methods
        public void AddVariant(ProductVariant variant)
        {
            _context.ProductVariants.Add(variant);
            _context.SaveChanges();
        }

        public void DeleteVariants(int productId)
        {
            // This is a bit tricky now since variants are linked to versions
            var variants = _context.ProductVariants.Where(v => v.ProductVersion.ProductId == productId).ToList();
            _context.ProductVariants.RemoveRange(variants);
            _context.SaveChanges();
        }

        // Specification methods
        public void AddSpecification(ProductSpecification spec)
        {
            _context.ProductSpecifications.Add(spec);
            _context.SaveChanges();
        }

        public void DeleteSpecifications(int productId)
        {
            var specs = _context.ProductSpecifications.Where(s => s.ProductVersion.ProductId == productId).ToList();
            _context.ProductSpecifications.RemoveRange(specs);
            _context.SaveChanges();
        }
    }
}
