using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Services.Infraestructure.AccountServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.AccountServices
{
    public class AuthenticationService : IAuthenticationService<SignInResult, IdentityResult, AppUser>
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthenticationService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed();

            return await _userManager.ConfirmEmailAsync(user, code);
        }

        public async Task RefreshSignInAsync(AppUser user)
        {
            await _signInManager.RefreshSignInAsync(user);
        }

        public async Task<IdentityResult> RegisterAsync(string email, string password)
        {
            var user = new AppUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<SignInResult> SignInAsync(string username, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return SignInResult.Failed;

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            return result;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
