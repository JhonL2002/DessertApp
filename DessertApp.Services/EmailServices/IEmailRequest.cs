
namespace DessertApp.Services.IEmailServices
{
    public interface IEmailRequest
    {
        string FromEmail { get; }
        string FromName { get; }
        string Subject { get; }
        string HtmlContent { get; }
        List<string> Recipients { get; }
    }
}
