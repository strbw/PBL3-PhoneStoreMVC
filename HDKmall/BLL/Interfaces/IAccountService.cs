using HDKmall.Models;
using HDKmall.ViewModels;

namespace HDKmall.BLL.Interfaces
{
    public interface IAccountService
    {
        User Authenticate(LoginVM model);
        bool RegisterUser(RegisterVM model);
    }
}