using DessertApp.Application.DessertServices;
using DessertApp.Application.IngredientServices;
using DessertApp.Application.MeasurementUnitServices;
using DessertApp.Application.Strategies;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace DessertApp.Application.InjectServices
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Ingredient Services
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IMeasurementUnitService, MeasurementUnitService>();

            //Dessert Services
            services.AddScoped<IDessertCategoryService, DessertCategoryService>();
            services.AddScoped(typeof(EmailServiceStrategy<>));
            return services;
        }
    }
}
