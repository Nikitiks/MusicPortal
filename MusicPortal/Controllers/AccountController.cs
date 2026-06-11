using Microsoft.AspNetCore.Mvc;
using MusicPortal.Common.Models;
using MusicPortal.BLL.Interfaces;
using Microsoft.Extensions.Localization;

namespace MusicPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IStringLocalizer<AccountController> _localizer;

        public AccountController(IUserService userService, IStringLocalizer<AccountController> localizer)
        {
            _userService = userService;
            _localizer = localizer;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.AuthenticateAsync(model);
                
                if (user == null)
                {
                    ViewBag.ErrorMessage = _localizer["InvalidCredentials"];
                    return View(model);
                }

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username ?? "");
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _userService.RegisterAsync(model);
                
                if (!success)
                {
                    ModelState.AddModelError("", _localizer["UserAlreadyExists"]);
                    return View(model);
                }

                ViewBag.SuccessMessage = _localizer["RegistrationSuccess"];
                return View();
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
