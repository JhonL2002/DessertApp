using DessertApp.Services.ConfigurationServices;
using DessertApp.Services.EmailServices;
using Mailjet.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.EmailServices
{
    public class MailjetClientFactory : IMailjetClientFactory<MailjetClient>
    {
        private readonly string _apiKey;
        private readonly string _secretKey;
        public MailjetClientFactory(IConfigurationFactory<IConfiguration> configurationFactory)
        {
            var configuration = configurationFactory.CreateConfiguration();
            _apiKey = configuration["EmailCredentials:ApiKey"]!;
            _secretKey = configuration["EmailCredentials:SecretKey"]!;
        }
        public MailjetClient CreateClient()
        {
            return new MailjetClient(_apiKey, _secretKey);
        }
    }
}
