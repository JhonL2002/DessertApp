using DessertApp.Application.DessertServices;
using DessertApp.Application.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace DessertApp.Application.ApplicationServicesInjections
{
    public static class ApplicationServiceInjections
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<CalculationService>();

            //Strategy for send email services
            services.AddScoped(typeof(EmailServiceStrategy<>));
            return services;
        }
    }
}
