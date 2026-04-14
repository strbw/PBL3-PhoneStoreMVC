using HDKmall.ViewModels;
using HDKmall.Models;

namespace HDKmall.BLL.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        Task<bool> AddProductAsync(ProductVM vm);
        Task<bool> UpdateProductAsync(int id, ProductVM vm);
        Task<bool> DeleteProductAsync(int id);
    }
}