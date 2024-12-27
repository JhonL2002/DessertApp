using DessertApp.Infraestructure.Data;
using DessertApp.Infraestructure.DataInitializerServices;
using DessertApp.Infraestructure.EmailServices;
using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Infraestructure.Repositories;
using DessertApp.Infraestructure.ResilienceServices;
using DessertApp.Infraestructure.RoleServices;
using DessertApp.Infraestructure.SecretServices;
using DessertApp.Infraestructure.UserServices;
using DessertApp.Services.ConfigurationServices;
using DessertApp.Services.DataInitializerServices;
using DessertApp.Services.EmailServices;
using DessertApp.Services.IEmailServices;
using DessertApp.Services.Repositories;
using DessertApp.Services.RoleStoreServices;
using DessertApp.Services.SecretServices;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
            //External services registration
            services.AddTransient<IManageSecrets, ManageSecrets>();

            //Configure resilience and database connection
            services.AddDbContext<AppDbContext>((provider ,options) =>
            {
                var secretProvider = provider.GetRequiredService<IManageSecrets>();
                string connectionString;

                try
                {
                    if (environment == "Development")
                    {
                        connectionString = configuration["SQL_CONNECTION_STRING"]!;
                    }
                    else
                    {
                        connectionString = secretProvider
                            .GetSecretsAsync("dessertkeyvault", "DessertAppSQL")
                            .GetAwaiter().GetResult();
                    }

                    options.UseSqlServer(connectionString, sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("DessertApp.Infraestructure");
                        sqlOptions.ExecutionStrategy(c => new CustomExecutionStrategies(c, 5, TimeSpan.FromSeconds(10)));
                    });
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error ocurred, see details: {ex}");
                }
                
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
            services.AddScoped<IUserStore<AppUser>, AppUserStore>();
            services.AddScoped<IRoleStore<AppRole>, AppRoleStore>();
            services.AddScoped<IExtendedRoleStore<AppRole>, AppRoleStore>();
            services.AddTransient<IConfigurationFactory<IConfiguration>, ConfigurationFactory>();

            //Add identity services (You need to add the implemented classes)
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            //Services for repositories
            services.AddScoped<IGenericRepository<AppRole, IdentityResult, string>, RoleRepository>();
            return services;
        }

        //Add external services instead databases (for example, Mailjet and Azure Key Vault)
        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            //Email sender services
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddTransient<IEmailRequestBuilder<MailjetRequest>, MailjetEmailRequestBuilder>();
            services.AddSingleton<IMailjetClientFactory<MailjetClient>, MailjetClientFactory>();

            //Add external key vault services (implemented Azure Key Vault)
            services.AddTransient<IManageSecrets, ManageSecrets>();
            return services;
        }

        //Add initialize data services to application
        public static async Task InitializeApplicationDataAsync(
            this IServiceCollection services,
            IConfiguration configuration,
            string environment,
            IServiceProvider serviceProvider)
        {
                var dataInitializer = serviceProvider.GetRequiredService<IDataInitializer>();
                var secretProvider = serviceProvider.GetRequiredService<IManageSecrets>();
                string adminEmail;

                if (environment == "Development")
                {
                    adminEmail = configuration["AdminUserCredentials:AdminEmail"]!;
                }
                else
                {
                    adminEmail = await secretProvider.GetSecretsAsync("dessertkeyvault", "DessertAdminApp");
                }

                await dataInitializer.InitializeRolesAsync();
                await dataInitializer.InitializeAdminUserAsync(adminEmail);
        }
    }
}
