using HDKmall.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HDKmall.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index(string? q)
        {
            ViewBag.ActiveTab = "users";
            var users = _accountService.GetAllUsers();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var key = q.Trim().ToLower();
                users = users.Where(u =>
                    (u.FullName ?? "").ToLower().Contains(key) ||
                    (u.Email ?? "").ToLower().Contains(key) ||
                    (u.PhoneNumber ?? "").ToLower().Contains(key)
                );
            }

            ViewBag.Search = q;
            ViewBag.Roles = _accountService.GetAllRoles();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            _accountService.ToggleUserStatus(id);
            TempData["Success"] = "Đã cập nhật trạng thái người dùng.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRole(int id, int roleId)
        {
            _accountService.ChangeUserRole(id, roleId);
            TempData["Success"] = "Đã cập nhật vai trò người dùng.";
            return RedirectToAction(nameof(Index));
        }
    }
}