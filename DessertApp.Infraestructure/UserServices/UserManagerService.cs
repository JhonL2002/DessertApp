using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Services.UserManagerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace DessertApp.Infraestructure.UserServices
{
    public class UserManagerService : IUserManagerService<IdentityResult, AppUser, IdentityOptions>
    {
        private readonly UserManager<AppUser> _userManager;

        public UserManagerService(UserManager<AppUser> userManager, IOptions<IdentityOptions> options)
        {
            _userManager = userManager;
            Options = options.Value;
        }

        public IdentityOptions? Options { get; set; }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed();

            return await _userManager.ConfirmEmailAsync(user, code);
        }

        public async Task<(IdentityResult result, AppUser user)> CreateUserAsync(string email, string password)
        {
            var user = new AppUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            return (result, user);
        }

        public async Task<AppUser?> FindByEmailAsync(string email)
        {
            if (email == null) return null;
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser?> FindByIdAsync(string userId)
        {
            if (userId == null) return null;
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
        {
            if (user == null) return null!;
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            if (user == null) return null!;
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<AppUser> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal  == null) return null!;
            var id = _userManager.GetUserId(claimsPrincipal);
            if (id != null)
            {
                return (await _userManager.FindByIdAsync(id))!;
            }
            return null!;
        }

        public async Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string newPassword)
        {
            if (user == null && token == null) return null!;
            return await _userManager.ResetPasswordAsync(user!, token, newPassword);
        }
    }
}
