using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Services.UserManagerServices;
using DessertApp.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace DessertApp.Controllers.Account
{
    public class ResetPasswordController : Controller
    {
        private IUserManagerService<IdentityResult, AppUser, IdentityOptions> _userManagerService;

        public ResetPasswordController(IUserManagerService<IdentityResult, AppUser, IdentityOptions> userManagerService)
        {
            _userManagerService = userManagerService;
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public IActionResult ResetUserPassword(ResetPasswordVM model, string code = null!)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                model.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetUserPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManagerService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation");
            }

            //Decode code before use
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));

            var result = await _userManagerService.ResetPasswordAsync(user, decodedCode, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
    }
}
