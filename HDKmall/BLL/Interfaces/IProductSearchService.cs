using HDKmall.ViewModels;
using HDKmall.Models;

namespace HDKmall.BLL.Interfaces
{
    public interface IProductSearchService
    {
        PaginationVM SearchProducts(ProductFilterVM filter);
        List<ProductListVM> GetProductsByCategory(int categoryId);
        List<ProductListVM> GetProductsByBrand(int brandId);
        List<ProductListVM> GetFeaturedProducts(int take = 10);
        List<ProductListVM> GetNewProducts(int take = 10);
        ProductDetailVM GetProductDetail(int id);
    }
}
