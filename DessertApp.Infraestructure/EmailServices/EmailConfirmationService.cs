using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Infraestructure.Utilities;
using DessertApp.Services.Enums;
using DessertApp.Services.Infraestructure.EmailServices;
using DessertApp.Services.Infraestructure.UserManagerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Text.Encodings.Web;

namespace DessertApp.Infraestructure.EmailServices
{
    public class EmailConfirmationService : IEmailSenderUrl<AppUser>
    {
        private readonly IUserManagerService<IdentityResult, AppUser, IdentityOptions> _userManagerService;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _environment;

        //Specify the type of implementation (to apply strategy implementation)
        public EmailServiceType ServiceType => EmailServiceType.EmailConfirmation;

        public EmailConfirmationService(
            IUserManagerService<IdentityResult, AppUser, IdentityOptions> userManagerService,
            IEmailSender emailSender,
            IWebHostEnvironment environment)
        {
            _userManagerService = userManagerService;
            _emailSender = emailSender;
            _environment = environment;
        }
        public async Task<string> GenerateUrlAsync(AppUser user, string returnUrl)
        {
            var code = await _userManagerService.GenerateEmailConfirmationTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string baseUrl = _environment.IsDevelopment()
                ? "https://localhost:443/RegisterUser/ConfirmEmail"
                : "https://dessert-app-jhonl2002-e3bnekfderdbejbr.brazilsouth-01.azurewebsites.net/RegisterUser/ConfirmEmail";

            return UrlHelperBuilder.BuildConfirmationUrl(baseUrl, user.Id, encodedCode, returnUrl);
        }


        public async Task SendEmailAsync(string email, string confirmationUrl)
        {
            await _emailSender.SendEmailAsync(
                email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationUrl)}'>clicking here</a>.");

        }
    }
}
