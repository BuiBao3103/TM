using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TM.Models;
using TM.Models.Entities;
using TM.Models.ViewModels;

namespace TM.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AccountManagementController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [RequireAuthorize("Admin")]
        public IActionResult Index()
        {
            var accounts = _context.Accounts
                .Select(a => new Models.ViewModels.AccountViewModel
                {
                    Id = a.Id,
                    UserName = a.Username,
                    Role = a.Role
                })
                .ToList();
            return View(accounts);
        }

        [RequireAuthorize("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAccountViewModel model)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin nhập vào.";
                    return RedirectToAction("Index");
                }

                // Check if username already exists
                var existingUser = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Username.ToLower() == model.UserName.ToLower());

                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Tên đăng nhập đã tồn tại trong hệ thống.";
                    return RedirectToAction("Index");
                }

                // Validate role
                if (model.Role != "Sale" && model.Role != "Admin")
                {
                    TempData["ErrorMessage"] = "Vai trò không hợp lệ.";
                    return RedirectToAction("Index");
                }

                // Create new account
                var newAccount = new Account
                {
                    Username = model.UserName.Trim(),
                    Password = HashPasswordBCrypt(model.Password), 
                    Role = model.Role,
                };

                _context.Accounts.Add(newAccount);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Tài khoản '{model.UserName}' đã được tạo thành công với vai trò {model.Role}.";
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                // Log error (you should use proper logging here)
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo tài khoản. Vui lòng thử lại.";
                return RedirectToAction("Index");
            }
        }

        [RequireAuthorize("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Json(new { exists = false });
            }

            var exists = await _context.Accounts
                .AnyAsync(a => a.Username.ToLower() == userName.ToLower());

            return Json(new { exists = exists });
        }

        private string HashPasswordBCrypt(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        private bool VerifyPasswordBCrypt(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        // Method để verify login (thêm vào sau)
        public bool ValidateLogin(string username, string password)
        {
            var user = _context.Accounts
                .FirstOrDefault(a => a.Username.ToLower() == username.ToLower());

            if (user == null) return false;

            return VerifyPasswordBCrypt(password, user.Password);
        }

    }
}