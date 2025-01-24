using DessertApp.Application.IngredientServices;
using DessertApp.Application.MeasurementUnitServices;
using DessertApp.Application.Strategies;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace DessertApp.Application.ApplicationServicesInjectors
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Ingredient Services
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IMeasurementUnitService, MeasurementUnitService>();
            services.AddScoped(typeof(EmailServiceStrategy<>));
            return services;
        }
    }
}
