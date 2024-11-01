
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace DessertApp.Services.IEmailServices
{
    public class MailjetEmailRequestBuilder : IEmailRequestBuilder<MailjetRequest>
    {
        private readonly MailjetRequest _request;
        public MailjetEmailRequestBuilder()
        {
            _request = new MailjetRequest
            {
                Resource = Send.Resource
            };
        }
        public IEmailRequestBuilder<MailjetRequest> AddRecipient(string recipientEmail)
        {
            _request.Property(Send.Recipients, new JArray { new JObject { { "Email", recipientEmail } } });
            return this;
        }

        public MailjetRequest Build()
        {
            return _request;
        }

        public IEmailRequestBuilder<MailjetRequest> SetFromEmail(string fromEmail)
        {
            _request.Property(Send.FromEmail, fromEmail);
            return this;
        }

        public IEmailRequestBuilder<MailjetRequest> SetFromName(string fromName)
        {
            _request.Property(Send.FromName, fromName);
            return this;
        }

        public IEmailRequestBuilder<MailjetRequest> SetHtmlContent(string htmlContent)
        {
            _request.Property(Send.HtmlPart, htmlContent);
            return this;
        }

        public IEmailRequestBuilder<MailjetRequest> SetSubject(string subject)
        {
            _request.Property(Send.Subject, subject);
            return this;
        }
    }
}
