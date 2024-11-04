using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.ConfigurationServices
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the configuration factory</typeparam>
    /// <typeparam name="TConfigurationInterface">The type of the configuration interface, typically IConfiguration </typeparam>
    public interface IConfigurationFactory<T,TConfigurationInterface> where T: class
    {
        /// <summary>
        /// Creates a new instance of the configuration of type <typeparamref name="TConfigurationInterface"/>
        /// </summary>
        /// <returns>A new instance of the configuration</returns>
        TConfigurationInterface CreateConfiguration();
    }
}
