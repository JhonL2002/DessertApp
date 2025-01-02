namespace DessertApp.Services.EmailServices
{
    public interface IEmailConfirmationService<T> where T : class
    {
        Task<string> GenerateConfirmationUrlAsync(T user, string returnUrl);
        Task SendConfirmationEmailAsync(string email, string confirmationUrl);
    }
}
