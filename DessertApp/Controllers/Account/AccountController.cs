using DessertApp.Services.AccountServices;
using DessertApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService<Microsoft.AspNetCore.Identity.SignInResult, IdentityResult> _authenticationService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAuthenticationService<Microsoft.AspNetCore.Identity.SignInResult, IdentityResult> authenticationService,
            ILogger<AccountController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null!)
        {
            //Pass returnUrl to view to handle post-login redirection
            ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login, string returnUrl = null!)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.SignInAsync(login.Email!,login.Password!, false, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("/Account/Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt, verify data");
                }
            }

            // Redisplay form with validation errors and input
            ViewData["ReturnUrl"] = returnUrl;
            return View(login);
        }
    }
}
