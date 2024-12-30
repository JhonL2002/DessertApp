using DessertApp.Infraestructure.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using DessertApp.Models.ViewModels;
using DessertApp.ViewModels;
using DessertApp.Services.UserManagerServices;

namespace DessertApp.Controllers.Account
{
    public class RegisterUserController : Controller
    {
        private readonly IUserManagerService<IdentityResult, AppUser, IdentityOptions> _userManagerService;
        private readonly ILogger<RegisterUserController> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterUserController(
            IUserManagerService<IdentityResult, AppUser, IdentityOptions> userManagerService,
            ILogger<RegisterUserController> logger,
            IEmailSender emailSender)
        {
            _userManagerService = userManagerService;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Register(RegisterVM model, string returnUrl = null!)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (result, user) = await _userManagerService.CreateUserAsync(model.Email, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password");
                var code = await _userManagerService.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = GenerateConfirmationUrl(user, code, returnUrl);

                try
                {
                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email");
                }

                if (_userManagerService.Options!.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToAction("RegisterConfirmation", "RegisterUser", new { email = model.Email, returnUrl});
                }
                return LocalRedirect(returnUrl);
            }
            return View(model);
        }

        public async Task<IActionResult> RegisterConfirmation(string email, string returnUrl)
        {
            if (email == null)
            {
                return View("/Index");
            }
            returnUrl ??= Url.Content("~/");

            var user = await _userManagerService.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            var confirmation = new RegisterConfirmationVM
            {
                Email = email,
                DisplayConfirmAccountLink = false,
                EmailConfirmationUrl = ""
            };
            return View(confirmation);
        }

        public async Task<IActionResult> ConfirmEmail(ConfirmEmailVM confirmEmail, string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManagerService.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManagerService.ConfirmEmailAsync(userId, code);
            confirmEmail.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return View(confirmEmail);
        }

        private string GenerateConfirmationUrl(AppUser user, string code, string returnUrl)
        {
            try
            {
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                return Url.Action(
                    action: "ConfirmEmail",
                    controller: "RegisterUser",
                    values: new { userId = user.Id, code, returnUrl },
                    protocol: Request.Scheme
                )!;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error ocurred, see details here: {ex}");
                return "";
            }
        }
    }
}
