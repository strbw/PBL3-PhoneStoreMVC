using HDKmall.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using HDKmall.Models;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace HDKmall.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetUserByEmail(string email)
        {
            // Include bảng Role để lấy RoleName
            return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.Include(u => u.Role).OrderByDescending(u => u.CreatedAt).ToList();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        // Thêm hàm lấy RoleID mặc định cho khách hàng
        public int GetCustomerRoleId()
        {
            var role = _context.Roles.FirstOrDefault(r => r.RoleName == "Customer");
            if (role != null) return role.RoleId;

            // Nếu chưa có, tự tạo role Customer
            var newRole = new Role { RoleName = "Customer" };
            _context.Roles.Add(newRole);
            _context.SaveChanges();
            return newRole.RoleId;
        }

        public List<UserAddress> GetUserAddressesByUserId(int userId)
        {
            return _context.UserAddresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .ToList();
        }

        public UserAddress GetUserAddressById(int userId, int addressId)
        {
            return _context.UserAddresses.FirstOrDefault(a => a.AddressId == addressId && a.UserId == userId);
        }

        public void AddUserAddress(UserAddress address)
        {
            _context.UserAddresses.Add(address);
        }

        public void DeleteUserAddress(UserAddress address)
        {
            _context.UserAddresses.Remove(address);
        }
        
        public IEnumerable<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
