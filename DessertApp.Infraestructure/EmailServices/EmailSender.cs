using DessertApp.Services.Infraestructure.ConfigurationServices;
using DessertApp.Services.Infraestructure.EmailServices;
using DessertApp.Services.Infraestructure.SecretServices;
using Mailjet.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace DessertApp.Infraestructure.EmailServices
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IMailjetClientFactory<MailjetClient,MailjetResponse,MailjetRequest> _mailjetClient;
        private readonly IEmailRequestBuilder<MailjetRequest> _emailRequestBuilder;
        private readonly IWebHostEnvironment _environment;
        private readonly IManageSecrets _secretManager;

        public string _fromEmail;


        public EmailSender(
            ILogger<EmailSender> logger,
            IEmailRequestBuilder<MailjetRequest> emailRequestBuilder,
            IConfigurationFactory<IConfiguration> configurationFactory,
            IMailjetClientFactory<MailjetClient, MailjetResponse, MailjetRequest> mailjetClient,
            IWebHostEnvironment environment,
            IManageSecrets secretManager)
        {
            _logger = logger;
            _emailRequestBuilder = emailRequestBuilder;
            _mailjetClient = mailjetClient;
            _environment = environment;
            _secretManager = secretManager;

            if (_environment.EnvironmentName == "Development")
            {
                //Additional configuration to create a custom configuration to read secrets (use it in development scenarios)
                //Read the FromEmail property to send email
                var configuration = configurationFactory.CreateConfiguration();
                _fromEmail = configuration["EmailSenderConfig:FromEmail"]!;
            }
            else
            {
                //Get the secret from secrets manager
                //Read the FROMEMAIL property to send email
                SetFromEmailFromVaultAsync().Wait();
            }
            
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
                var response = await _mailjetClient.SendEmailAsync(request);
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

        private async Task SetFromEmailFromVaultAsync()
        {
            _fromEmail = await _secretManager.GetSecretsAsync("dessertkeyvault", "FROMEMAIL");

        }
    }
}
