using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.UserManagerServices
{
    public interface IUserManagerService<TResult, T, TOptions> where T : class
    {
        Task<(TResult result, T user)> CreateUserAsync(string email, string password);
        Task<TResult> ConfirmEmailAsync(string userId, string code);
        Task<T?> FindByEmailAsync(string email);
        Task<T?> FindByIdAsync(string userId);
        Task<string> GenerateEmailConfirmationTokenAsync(T user);
        TOptions? Options { get; set; }
    }
}
