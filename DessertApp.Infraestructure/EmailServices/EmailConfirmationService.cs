using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Infraestructure.Utilities;
using DessertApp.Services.EmailServices;
using DessertApp.Services.UserManagerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Text.Encodings.Web;

namespace DessertApp.Infraestructure.EmailServices
{
    public class EmailConfirmationService : IEmailConfirmationService<AppUser>
    {
        private readonly IUserManagerService<IdentityResult, AppUser, IdentityOptions> _userManagerService;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _environment;

        public EmailConfirmationService(
            IUserManagerService<IdentityResult, AppUser, IdentityOptions> userManagerService,
            IEmailSender emailSender,
            IWebHostEnvironment environment)
        {
            _userManagerService = userManagerService;
            _emailSender = emailSender;
            _environment = environment;
        }
        public async Task<string> GenerateConfirmationUrlAsync(AppUser user, string returnUrl)
        {
            var code = await _userManagerService.GenerateEmailConfirmationTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string baseUrl = _environment.IsDevelopment()
                ? "https://localhost:7282/RegisterUser/ConfirmEmail"
                : "https://setdomainhere";

            return UrlHelperBuilder.BuildConfirmationUrl(baseUrl, user.Id, encodedCode, returnUrl);
        }


        public async Task SendConfirmationEmailAsync(string email, string confirmationUrl)
        {
            await _emailSender.SendEmailAsync(
                email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationUrl)}'>clicking here</a>.");

        }
    }
}
