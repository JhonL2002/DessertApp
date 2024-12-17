using DessertApp.Infraestructure.Data;
using DessertApp.Infraestructure.DataInitializerServices;
using DessertApp.Infraestructure.EmailServices;
using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Infraestructure.Repositories;
using DessertApp.Infraestructure.ResilienceServices;
using DessertApp.Infraestructure.RoleServices;
using DessertApp.Infraestructure.UserServices;
using DessertApp.Models.IdentityModels;
using DessertApp.Services.ConfigurationServices;
using DessertApp.Services.DataInitializerServices;
using DessertApp.Services.EmailServices;
using DessertApp.Services.IEmailServices;
using DessertApp.Services.Repositories;
using DessertApp.Services.RoleStoreServices;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DessertApp.Infraestructure.ConfigurationServices
{
    public static class InfraestructureServiceExtensions
    {
        public static IServiceCollection AddInfraestructureServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string environment)
        {
            var connectionString = environment == "Development"
                    ? configuration["SQL_CONNECTION_STRING"]
                    : configuration.GetConnectionString("AzureSQL");

            //Validate connection string
            if (!IsValidConnectionString(connectionString) || connectionString == null)
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    throw new InvalidOperationException("Invalid Connection string, verify the connection string");
                });
                return services;
            }

            //Configure resilience to database connection
            services.AddDbContext<AppDbContext>(options =>
            {
                    options.UseSqlServer(connectionString, sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("DessertApp.Infraestructure");
                        /*sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);*/
                        sqlOptions.ExecutionStrategy(c => new CustomExecutionStrategies(c, 5, TimeSpan.FromSeconds(10)));
                    });
            });

            //Add filters to logging services
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Connection", LogLevel.Warning);
                logging.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);
            });
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Initialize Data services
            services.AddScoped<IDataInitializer, DataInitializer>();

            //Application services
            services.AddScoped<IUserStore<IAppUser>, AppUserStore>();
            services.AddScoped<IRoleStore<IAppRole>, AppRoleStore>();
            services.AddScoped<IExtendedRoleStore<IAppRole>, AppRoleStore>();
            services.AddTransient<IConfigurationFactory<IConfiguration>, ConfigurationFactory>();

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            //Repositories
            services.AddScoped<IGenericRepository<IAppRole, IdentityResult, string>, RoleRepository>();
            return services;
        }

        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            //Email sender services
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IEmailRequestBuilder<MailjetRequest>, MailjetEmailRequestBuilder>();
            services.AddSingleton<IMailjetClientFactory<MailjetClient>, MailjetClientFactory>();
            return services;
        }

        private static bool IsValidConnectionString(string? connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Cannot connect to database. See details: {ex.Message}");
                return false;
            }
        }
    }
}
