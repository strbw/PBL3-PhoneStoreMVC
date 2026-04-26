using HDKmall.Models;
using HDKmall.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HDKmall.BLL.Interfaces
{
    public interface IWishlistService
    {
        Task<bool> ToggleWishlistAsync(int userId, int productId, int? versionId);
        Task<List<ProductListVM>> GetUserWishlistAsync(int userId);
        Task<bool> IsInWishlistAsync(int userId, int productId, int? versionId);
        Task<int> GetWishlistCountAsync(int userId);
    }
}
