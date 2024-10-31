using Mailjet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.EmailServices
{
    public interface IEmailRequestBuilder
    {
        IEmailRequestBuilder SetFromEmail(string fromEmail);
        IEmailRequestBuilder SetFromName(string fromName);
        IEmailRequestBuilder SetSubject(string subject);
        IEmailRequestBuilder SetHtmlContent(string htmlContent);
        IEmailRequestBuilder AddRecipient(string recipientEmail);
        MailjetRequest Build();
    }
}
