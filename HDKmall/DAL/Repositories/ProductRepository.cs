using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using Microsoft.EntityFrameworkCore;

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
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .Include(p => p.Specifications)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefault(p => p.Id == id);
        }

        public Product GetBySlug(string slug)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .Include(p => p.Specifications)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
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
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Include(p => p.Specifications)
                .FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
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
            var variants = _context.ProductVariants.Where(v => v.ProductId == productId).ToList();
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
            var specs = _context.ProductSpecifications.Where(s => s.ProductId == productId).ToList();
            _context.ProductSpecifications.RemoveRange(specs);
            _context.SaveChanges();
        }
    }
}
