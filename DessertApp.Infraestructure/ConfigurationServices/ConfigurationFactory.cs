using DessertApp.Infraestructure.Data;
using DessertApp.Services.ConfigurationServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
