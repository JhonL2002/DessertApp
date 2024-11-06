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
    public interface IConfigurationFactory<T>
    {
        /// <summary>
        /// Creates a new instance of the configuration of type <typeparamref name="T"/>
        /// </summary>
        /// <returns>A new instance of the configuration</returns>
        T CreateConfiguration();
    }
}
