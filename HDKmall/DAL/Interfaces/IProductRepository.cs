using HDKmall.Models;

namespace HDKmall.DAL.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);

        // Image methods
        void AddImage(ProductImage image);
        void DeleteImage(int imageId);

        // Variant methods
        void AddVariant(ProductVariant variant);
        void DeleteVariants(int productId);

        // Specification methods
        void AddSpecification(ProductSpecification spec);
        void DeleteSpecifications(int productId);
    }
}