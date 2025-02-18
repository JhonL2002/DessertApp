namespace DessertApp.Services.Infraestructure.SecretServices
{
    public interface IManageSecrets
    {
        Task<string> GetSecretsAsync(string keyVaultName, string secretName);
    }
}
