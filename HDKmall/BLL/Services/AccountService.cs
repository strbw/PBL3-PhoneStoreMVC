using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using HDKmall.ViewModels;
namespace HDKmall.BLL.Services
{   
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Authenticate(LoginVM model)
        {
            var user = _userRepository.GetUserByEmail(model.Email);
            if (user == null || string.IsNullOrEmpty(user.PasswordHash)) return null;

            // Kiểm tra mật khẩu (hỗ trợ cả mã hóa BCrypt và plain text từ DB)
            if (user.PasswordHash == model.Password)
            {
                return user; // Mật khẩu lưu dưới dạng plain text trong DB
            }

            try 
            {
                bool isValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
                return isValid ? user : null;
            }
            catch
            {
                // Nếu PasswordHash trong DB không phải là chuỗi mã hóa BCrypt hợp lệ, hàm Verify sẽ văng lỗi.
                return null; 
            }
        }

        public bool RegisterUser(RegisterVM model)
        {
            var existingUser = _userRepository.GetUserByEmail(model.Email);
            if (existingUser != null) return false;

            var newUser = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                RoleId = _userRepository.GetCustomerRoleId(), // Lấy RoleID của Customer
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _userRepository.AddUser(newUser);
            _userRepository.SaveChanges();
            return true;
        }
    }
}