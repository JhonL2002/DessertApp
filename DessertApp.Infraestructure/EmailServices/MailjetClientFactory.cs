using DessertApp.Services.ConfigurationServices;
using DessertApp.Services.EmailServices;
using DessertApp.Services.SecretServices;
using Mailjet.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.EmailServices
{
    public class MailjetClientFactory : IMailjetClientFactory<MailjetClient, MailjetResponse, MailjetRequest>
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IManageSecrets _manageSecrets;
        private readonly IConfigurationFactory<IConfiguration> _configurationFactory;

        private string? _apiKey;
        private string? _secretKey;

        public MailjetClientFactory(
            IConfigurationFactory<IConfiguration> configurationFactory,
            IWebHostEnvironment environment,
            IManageSecrets manageSecrets)
        {
            _environment = environment;
            _manageSecrets = manageSecrets;
            _configurationFactory = configurationFactory;

        }
        public async Task<MailjetClient> CreateClientAsync()
        {
            if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_secretKey))
            {
                await InitializeSecretsAsync();
            }
            return new MailjetClient(_apiKey, _secretKey);
        }

        public async Task InitializeSecretsAsync()
        {
            if (_environment.EnvironmentName == "Development")
            {
                var configuration = _configurationFactory.CreateConfiguration();
                _apiKey = configuration["EmailCredentials:ApiKey"];
                _secretKey = configuration["EmailCredentials:SecretKey"];
            }
            else
            {
                _apiKey = await _manageSecrets.GetSecretsAsync("dessertkeyvault","APIKEY");
                _secretKey = await _manageSecrets.GetSecretsAsync("dessertkeyvault", "SECRETKEY");
            }
        }

        public async Task<MailjetResponse> SendEmailAsync(MailjetRequest request)
        {
            var client = await CreateClientAsync();
            return await client.PostAsync(request);
        }
    }
}
