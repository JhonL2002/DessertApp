using DessertApp.Infraestructure.AccountServices;
using DessertApp.Infraestructure.Data;
using DessertApp.Infraestructure.DataInitializerServices;
using DessertApp.Infraestructure.EmailServices;
using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Infraestructure.Repositories;
using DessertApp.Infraestructure.ResilienceServices;
using DessertApp.Infraestructure.RoleServices;
using DessertApp.Infraestructure.SecretServices;
using DessertApp.Infraestructure.UnitOfWorkServices;
using DessertApp.Infraestructure.UserServices;
using DessertApp.Services.AccountServices;
using DessertApp.Services.ConfigurationServices;
using DessertApp.Services.DataInitializerServices;
using DessertApp.Services.EmailServices;
using DessertApp.Services.IEmailServices;
using DessertApp.Services.Repositories;
using DessertApp.Services.RepositoriesServices;
using DessertApp.Services.RoleStoreServices;
using DessertApp.Services.SecretServices;
using DessertApp.Services.UnitOfWorkServices;
using DessertApp.Services.UserManagerServices;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DessertApp.Infraestructure.ConfigurationServices
{
    public static class InfraestructureServiceExtensions
    {
        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string environment)
        {

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
            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            //Initialize Data services (Initialize services for AppRole)
            services.AddScoped<IDataInitializer, DataInitializer>();

            //Application services
            services.AddScoped<IUserStore<AppUser>, AppUserStore>();
            services.AddScoped<IUserRoleStore<AppUser>, AppUserStore>();
            services.AddScoped<IUserPhoneNumberStore<AppUser>, AppUserStore>();
            services.AddScoped<IRoleStore<AppRole>, AppRoleStore>();
            services.AddScoped<IExtendedRoleStore<AppRole>, AppRoleStore>();

            //Add identity services (You need to add the implemented classes)
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            //Add external users manager (implemented Identity from Entity Framework)
            services.AddScoped<IUserManagerService<IdentityResult, AppUser, IdentityOptions>, UserManagerService>();

            //Add external authentication services (implemented Identity from Entity Framework)
            services.AddScoped<IAuthenticationService<SignInResult, IdentityResult, AppUser>, AuthenticationService>();
            return services;
        }

        public static IServiceCollection AddRepositoriesServices(this IServiceCollection services)
        {
            //Services for repositories
            services.AddScoped<IGenericIdentityRepository<AppRole, IdentityResult, string>, RoleRepository>();
            services.AddScoped(typeof(IDomainGenericRepository<,>), typeof(DomainPersistentRepository<,>));

            //UnitOfWork services
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        //Add external services instead databases (for example, Mailjet and Azure Key Vault)
        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            //Email sender services
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddTransient<IEmailRequestBuilder<MailjetRequest>, MailjetEmailRequestBuilder>();
            services.AddSingleton<IMailjetClientFactory<MailjetClient, MailjetResponse, MailjetRequest>, MailjetClientFactory>();
            services.AddScoped<IEmailConfirmationService<AppUser>,  EmailConfirmationService>();

            //Add external key vault services (implemented Azure Key Vault)
            services.AddTransient<IManageSecrets, ManageSecrets>();

            return services;
        }

        public static IServiceCollection AddConfigurationServices(this IServiceCollection services)
        {
            services.AddTransient<IConfigurationFactory<IConfiguration>, ConfigurationFactory>();
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
