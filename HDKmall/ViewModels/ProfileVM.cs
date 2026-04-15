using System.ComponentModel.DataAnnotations;

namespace HDKmall.ViewModels
{
    public class ProfileVM
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(0[3-9])\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }

        public string? NewAddressRecipientName { get; set; }
        public string? NewAddressPhoneNumber { get; set; }
        public string? NewAddressLine { get; set; }
        public bool SetAsDefaultAddress { get; set; }

        public List<AddressItemVM> Addresses { get; set; } = new();
    }

    public class AddressItemVM
    {
        public int AddressId { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}
