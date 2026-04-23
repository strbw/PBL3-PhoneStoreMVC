using HDKmall.Models;
using HDKmall.ViewModels;

namespace HDKmall.BLL.Interfaces
{
    public interface IAccountService
    {
        User Authenticate(LoginVM model);
        bool RegisterUser(RegisterVM model);
        ProfileVM GetProfile(int userId);
        bool UpdateProfile(int userId, ProfileVM model);
        bool ChangePassword(int userId, string currentPassword, string newPassword);
        bool AddAddress(int userId, ProfileVM model);
        bool DeleteAddress(int userId, int addressId);
        bool SetDefaultAddress(int userId, int addressId);
        IEnumerable<User> GetAllUsers();
        void ToggleUserStatus(int userId);
        void ChangeUserRole(int userId, int roleId);
        IEnumerable<Role> GetAllRoles();
    }
}
