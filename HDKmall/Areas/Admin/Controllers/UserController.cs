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
            return View(users);
        }
    }
}