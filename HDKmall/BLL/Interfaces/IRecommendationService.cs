using HDKmall.ViewModels;
using System.Collections.Generic;

namespace HDKmall.BLL.Interfaces
{
    public interface IRecommendationService
    {
        // Recently Viewed
        void AddToRecentlyViewed(int productId);
        List<ProductListVM> GetRecentlyViewedProducts(int take = 10);

        // Recent Searches
        void AddToRecentSearches(string query);
        List<string> GetRecentSearches(int take = 5);

        // Recommendations based on context (Product Detail or Cart)
        List<ProductListVM> GetRelatedProducts(int productId, int take = 8);
        List<ProductListVM> GetRecommendationsForCart(List<int> productIdsInCart, int take = 10);
        
        // General recommendations (For Search bar or Home)
        List<ProductListVM> GetPersonalizedRecommendations(int take = 10);
    }
}
