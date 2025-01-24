using DessertApp.Application.Strategies;
using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Services.Enums;
using DessertApp.Services.UserManagerServices;
using DessertApp.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers.Account
{
    public class ForgotPasswordController : Controller
    {
        private readonly IUserManagerService<IdentityResult, AppUser, IdentityOptions> _userManagerService;
        private readonly EmailServiceStrategy<AppUser> _emailServiceStrategy;

        public ForgotPasswordController(
            IUserManagerService<IdentityResult, AppUser, IdentityOptions> userManagerService,
            EmailServiceStrategy<AppUser> emailServiceStrategy)
        {
            _userManagerService = userManagerService;
            _emailServiceStrategy = emailServiceStrategy;
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        public IActionResult SendResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPassword(UserEmailVM userEmail)
        {
            //Call the adequate service (applying strategy)
            var emailService = _emailServiceStrategy.GetService(EmailServiceType.ResetPassword);

            //Get the user by email
            var user = await _userManagerService.FindByEmailAsync(userEmail.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            //Generate URL to reset password
            var resetPasswordUrl = await emailService.GenerateUrlAsync(user, "ResetPassword/ResetUserPassword");

            //Send reset password email
            await emailService.SendEmailAsync(userEmail.Email, resetPasswordUrl);

            return RedirectToAction("ForgotPasswordConfirmation");
        }
    }
}
