
namespace DessertApp.Services.SecretServices
{
    public interface IManageSecrets
    {
        Task<string> GetSecretsAsync(string keyVaultName, string secretName);
    }
}
