using DessertApp.Services.Enums;

namespace DessertApp.Services.EmailServices
{
    public interface IEmailSenderUrl<T> where T : class
    {
        EmailServiceType ServiceType { get; }
        Task<string> GenerateUrlAsync(T user, string returnUrl);
        Task SendEmailAsync(string email, string confirmationUrl);
    }
}
