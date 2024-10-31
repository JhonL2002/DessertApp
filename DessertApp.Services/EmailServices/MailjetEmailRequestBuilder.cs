using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.EmailServices
{
    public class MailjetEmailRequestBuilder : IEmailRequestBuilder
    {
        private readonly MailjetRequest _request;
        public MailjetEmailRequestBuilder()
        {
            _request = new MailjetRequest
            {
                Resource = Send.Resource
            };
        }
        public IEmailRequestBuilder AddRecipient(string recipientEmail)
        {
            _request.Property(Send.Recipients, new JArray { new JObject { { "Email", recipientEmail } } });
            return this;
        }

        public MailjetRequest Build()
        {
            return _request;
        }

        public IEmailRequestBuilder SetFromEmail(string fromEmail)
        {
            _request.Property(Send.FromEmail, fromEmail);
            return this;
        }

        public IEmailRequestBuilder SetFromName(string fromName)
        {
            _request.Property(Send.FromName, fromName);
            return this;
        }

        public IEmailRequestBuilder SetHtmlContent(string htmlContent)
        {
            _request.Property(Send.HtmlPart, htmlContent);
            return this;
        }

        public IEmailRequestBuilder SetSubject(string subject)
        {
            _request.Property(Send.Subject, subject);
            return this;
        }
    }
}
