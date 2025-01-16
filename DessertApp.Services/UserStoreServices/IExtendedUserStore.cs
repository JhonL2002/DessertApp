using System.Security.Claims;

namespace DessertApp.Services.UserStoreServices
{
    public interface IExtendedUserStore<TResult, T, TOptions> where T : class
    {
        Task<(TResult result, T user)> CreateUserAsync(T appUser, string password);
        Task<TResult> ConfirmEmailAsync(string userId, string code);
        Task<T?> FindByEmailAsync(string email);
        Task<T?> FindByIdAsync(string userId);
        Task<string> GenerateEmailConfirmationTokenAsync(T user);
        Task<T> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        TOptions? Options { get; set; }
    }
}
