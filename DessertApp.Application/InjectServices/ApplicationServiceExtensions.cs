using DessertApp.Application.IngredientServices;
using DessertApp.Application.MeasurementUnitServices;
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
            return services;
        }
    }
}
