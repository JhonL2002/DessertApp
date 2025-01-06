using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DessertApp.Services.SecretServices;
using Microsoft.Extensions.Logging;

namespace DessertApp.Infraestructure.SecretServices
{
    public class ManageSecrets : IManageSecrets
    {
        private readonly ILogger<ManageSecrets> _logger;

        public ManageSecrets(ILogger<ManageSecrets> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetSecretsAsync(string keyVaultName, string secretName)
        {
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            try
            {
                // Usamos EnvironmentCredential que ya buscará las variables de entorno
                var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
                var secret = await client.GetSecretAsync(secretName);
                return secret.Value.Value;
            }
            catch (Exception ex)
            {
                // Loguear la excepción para poder diagnosticarla
                _logger.LogCritical($"Error to get secret: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

    }
}
