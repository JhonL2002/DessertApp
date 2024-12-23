using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DessertApp.Services.SecretServices;

namespace DessertApp.Infraestructure.SecretServices
{
    public class ManageSecrets : IManageSecrets
    {
        public async Task<string> GetSecretsAsync(string keyVaultName,string secretName)
        {
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var secret = await client.GetSecretAsync(secretName);
            return secret.Value.Value;
        }
    }
}
