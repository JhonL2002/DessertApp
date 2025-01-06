using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.EmailServices
{
    public interface IMailjetClientFactory<T,TResponse,TRequest>
    {
        Task InitializeSecretsAsync();
        /// <summary>
        /// Creates a new instance of the email client service to implement <typeparamref name="T"/>
        /// </summary>
        /// <returns>A new instance of the email client service to create</returns>
        Task<T> CreateClientAsync();
        /// <summary>
        /// Send an email after creating the email client
        /// </summary>
        /// <param name="request">The <typeparamref name="TRequest"/> to send email </param>
        /// <returns>A <typeparamref name="TResponse"/> that performs a PostAsync action</returns>
        Task<TResponse> SendEmailAsync(TRequest request);
    }
}
