using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.AccountServices
{
    public interface IAuthenticationService<TSignInResult, TResult>
    {
        Task<TSignInResult> SignInAsync(string username, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignOutAsync();
        Task<TResult> RegisterAsync(string email, string password);
        Task<TResult> ConfirmEmailAsync(string userId, string code);
    }
}
