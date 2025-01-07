using DessertApp.Infraestructure.Data;
using DessertApp.Services.ConfigurationServices;
using Microsoft.Extensions.Configuration;

namespace DessertApp.Infraestructure.ConfigurationServices
{
    public class ConfigurationFactory : IConfigurationFactory<IConfiguration>
    {
        public IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<AppDbContextFactory>()
                .Build();
        }
    }
}
