
namespace DessertApp.Services.AccountServices
{
    public interface IAuthenticationService<TSignInResult, TResult,T>
    {
        Task<TSignInResult> SignInAsync(string username, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignOutAsync();
        Task<TResult> RegisterAsync(string email, string password);
        Task RefreshSignInAsync(T user);
        Task<TResult> ConfirmEmailAsync(string userId, string code);
    }
}
