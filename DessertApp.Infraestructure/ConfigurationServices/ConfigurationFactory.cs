using DessertApp.Services.ConfigurationServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.ConfigurationServices
{
    public class ConfigurationFactory<T> : IConfigurationFactory<T, IConfiguration> where T : class
    {
        public IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddUserSecrets<T>()
                .Build();
        }
    }
}
