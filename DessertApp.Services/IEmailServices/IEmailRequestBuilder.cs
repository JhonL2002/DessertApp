
namespace DessertApp.Services.IEmailServices
{
    public interface IEmailRequestBuilder<T>
    {
        IEmailRequestBuilder<T> SetFromEmail(string fromEmail);
        IEmailRequestBuilder<T> SetFromName(string fromName);
        IEmailRequestBuilder<T> SetSubject(string subject);
        IEmailRequestBuilder<T> SetHtmlContent(string htmlContent);
        IEmailRequestBuilder<T> AddRecipient(string recipientEmail);
        T Build();
    }
}
