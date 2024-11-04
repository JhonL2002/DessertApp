using DessertApp.Services.IEmailServices;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace DessertApp.Infraestructure.EmailServices
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;
        private readonly IMailjetClient _mailjetClient;
        private readonly IEmailRequestBuilder<MailjetRequest> _emailRequestBuilder;
        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger, IMailjetClient mailjetClient, IEmailRequestBuilder<MailjetRequest> emailRequestBuilder)
        {
            _configuration = configuration;
            _logger = logger;
            _mailjetClient = mailjetClient;
            _emailRequestBuilder = emailRequestBuilder;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                //Build email content to send
                //The FromEmail property added at secrets.json of DessertApp Project
                var request = _emailRequestBuilder
                    .SetFromEmail(_configuration["EmailSenderConfig:FromEmail"]!)
                    .SetFromName("Application Team")
                    .SetSubject(subject)
                    .SetHtmlContent(htmlMessage)
                    .AddRecipient(email)
                    .Build();

                //Send the email to recipient
                var response = await _mailjetClient.PostAsync(request);

                //Show logs for response result
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Email sent successfully to {email}");
                }
                else
                {
                    _logger.LogError($"Failed to send email to {email}. StatusCode {response.StatusCode}, Error: {response.GetErrorMessage()}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email: {ex.Message}");
                throw;
            }

        }
    }
}
