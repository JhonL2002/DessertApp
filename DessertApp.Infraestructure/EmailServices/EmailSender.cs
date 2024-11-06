using DessertApp.Services.ConfigurationServices;
using DessertApp.Services.EmailServices;
using DessertApp.Services.IEmailServices;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace DessertApp.Infraestructure.EmailServices
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly MailjetClient _mailjetClient;
        private readonly IEmailRequestBuilder<MailjetRequest> _emailRequestBuilder;
        public string _fromEmail;
        public EmailSender(
            ILogger<EmailSender> logger,
            IEmailRequestBuilder<MailjetRequest> emailRequestBuilder,
            IConfigurationFactory<IConfiguration> configurationFactory,
            IMailjetClientFactory<MailjetClient> mailjetClient)
        {
            _logger = logger;
            _emailRequestBuilder = emailRequestBuilder;
            _mailjetClient = mailjetClient.CreateClient() ?? throw new InvalidOperationException("No se pudo crear MailjetClient."); ;

            //Additional configuration to create a custom configuration to read secrets
            var configuration = configurationFactory.CreateConfiguration();
            _fromEmail = configuration["EmailSenderConfig:FromEmail"]! ?? throw new ArgumentNullException(nameof(_fromEmail), "El valor de 'FromEmail' no puede ser nulo.");
            
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                //Build email content to send
                var request = _emailRequestBuilder
                    .SetFromEmail(_fromEmail)
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
