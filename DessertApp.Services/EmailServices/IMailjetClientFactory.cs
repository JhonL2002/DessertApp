using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.EmailServices
{
    public interface IMailjetClientFactory<T>
    {
        /// <summary>
        /// Creates a new instance of the email client service to implement <typeparamref name="T"/>
        /// </summary>
        /// <returns>A new instance of the email client service to create</returns>
        T CreateClient();
    }
}
