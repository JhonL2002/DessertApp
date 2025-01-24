using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Services.AccountServices;
using DessertApp.Services.UserManagerServices;
using DessertApp.ViewModels.AccountVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService<Microsoft.AspNetCore.Identity.SignInResult, IdentityResult, AppUser> _authenticationService;
        private readonly IUserManagerService<IdentityResult, AppUser, IdentityOptions> _userManagerService;
        private readonly IUserPhoneNumberStore<AppUser> _userPhone;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAuthenticationService<Microsoft.AspNetCore.Identity.SignInResult, IdentityResult, AppUser> authenticationService,
            IUserManagerService<IdentityResult, AppUser, IdentityOptions> userManagerService,
            IUserPhoneNumberStore<AppUser> userPhone,
            ILogger<AccountController> logger)
        {
            _authenticationService = authenticationService;
            _userManagerService = userManagerService;
            _userPhone = userPhone;
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

        // GET: Account/Edit/CurrentUser
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManagerService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }
            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> Edit(UserDataVM user)
        {
            var foundUser = await _userManagerService.GetUserAsync(User);
            if (foundUser == null)
            {
                return NotFound($"Unable to load user.");
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var phoneNumber = await _userPhone.GetPhoneNumberAsync(foundUser, CancellationToken.None);
            if (user.PhoneNumber != phoneNumber)
            {
                await _userPhone.SetPhoneNumberAsync(foundUser, user.PhoneNumber, CancellationToken.None);
            }

            await _authenticationService.RefreshSignInAsync(foundUser);
            user.StatusMessage = "Your profile has been updated";
            return View(user);
        }
    }
}
