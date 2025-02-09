using DessertApp.Services.EmailServices;
using DessertApp.Services.Enums;
namespace DessertApp.Application.Strategies
{
    public class EmailServiceStrategy<T> where T : class
    {
        private readonly IEnumerable<IEmailSenderUrl<T>> _emailServices;

        public EmailServiceStrategy(IEnumerable<IEmailSenderUrl<T>> emailServices)
        {
            _emailServices = emailServices;
        }

        public IEmailSenderUrl<T> GetService(EmailServiceType serviceType)
        {
            var service = _emailServices.FirstOrDefault(s => s.ServiceType == serviceType);
            if (service == null)
            {
                throw new KeyNotFoundException($"No email service found for type '{serviceType}'");
            }
            return service;
        }
    }
}
