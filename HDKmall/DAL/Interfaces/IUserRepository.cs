using HDKmall.Models;
namespace HDKmall.DAL.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        User GetUserById(int userId);
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        void SaveChanges();
        int GetCustomerRoleId();
        List<UserAddress> GetUserAddressesByUserId(int userId);
        UserAddress GetUserAddressById(int userId, int addressId);
        void AddUserAddress(UserAddress address);
        void DeleteUserAddress(UserAddress address);
    }
}
