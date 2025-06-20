using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using TM.Models;
using TM.Models.Entities;
using TM.Models.ViewModels;

namespace TM.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            Account? user = _context.Accounts.FirstOrDefault(a =>
                a.Username == model.Username &&
                a.Password == model.Password);

            if (user == null)
            {
                model.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View(model);
            }

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("AuthId", user.Id.ToString());
            return RedirectToAction("Index", "Tour");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }



        // user profile
        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var user = _context.Accounts.FirstOrDefault(a => a.Username == username);
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string OldPassword, string NewPassword)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

            var account = _context.Accounts.FirstOrDefault(a => a.Username == username);
            if (account == null || account.Password != OldPassword)
            {
                TempData["ChangePassStatus"] = "error";
                return RedirectToAction("Profile");
            }

            account.Password = NewPassword;
            _context.SaveChanges();

            TempData["ChangePassStatus"] = "success";
            return RedirectToAction("Profile");
        }

    }

    public class RequireAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public RequireAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            String? role = context.HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || !_roles.Contains(role))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
        }
    }

}
