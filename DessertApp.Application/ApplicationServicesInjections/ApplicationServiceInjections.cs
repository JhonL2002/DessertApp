using DessertApp.Application.BackgroundWorkers;
using DessertApp.Application.DessertServices;
using DessertApp.Application.InventoryServices;
using DessertApp.Application.Strategies;
using DessertApp.Services.Application.DessertServices;
using Microsoft.Extensions.DependencyInjection;

namespace DessertApp.Application.ApplicationServicesInjections
{
    public static class ApplicationServiceInjections
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDessertDemandService, DessertDemandService>();

            services.AddScoped<DessertCalculationService>();
            services.AddScoped<StockCheckerService>();

            //Strategy for send email services
            services.AddScoped(typeof(EmailServiceStrategy<>));

            services.AddHostedService<ReplenishmentWorker>();
            return services;
        }
    }
}
