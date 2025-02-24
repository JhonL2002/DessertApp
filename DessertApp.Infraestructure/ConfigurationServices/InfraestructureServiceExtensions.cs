using AspNetCoreRateLimit;
using DessertApp.Infraestructure.AccountServices;
using DessertApp.Infraestructure.CacheServices;
using DessertApp.Infraestructure.Data;
using DessertApp.Infraestructure.DataInitializerServices;
using DessertApp.Infraestructure.EmailServices;
using DessertApp.Infraestructure.EntityRepositories;
using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Infraestructure.ImportDataServices;
using DessertApp.Infraestructure.InfraestructureRepositories;
using DessertApp.Infraestructure.ResilienceServices;
using DessertApp.Infraestructure.RoleServices;
using DessertApp.Infraestructure.SecretServices;
using DessertApp.Infraestructure.UnitOfWorkServices;
using DessertApp.Infraestructure.UserServices;
using DessertApp.Models.Entities;
using DessertApp.Services.DTOs;
using DessertApp.Services.Infraestructure.AccountServices;
using DessertApp.Services.Infraestructure.CacheServices;
using DessertApp.Services.Infraestructure.ConfigurationServices;
using DessertApp.Services.Infraestructure.DataInitializerServices;
using DessertApp.Services.Infraestructure.EmailServices;
using DessertApp.Services.Infraestructure.ImportDataServices;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.RepositoriesServices.IdentityRepositories;
using DessertApp.Services.Infraestructure.RoleStoreServices;
using DessertApp.Services.Infraestructure.SecretServices;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using DessertApp.Services.Infraestructure.UserManagerServices;
using Hangfire;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DessertApp.Infraestructure.ConfigurationServices
{
    public static class InfraestructureServiceExtensions
    {
        static string connectionString = string.Empty;

        private static string GetConnectionString(IConfiguration configuration, IServiceProvider provider, string environment)
        {
            var secretProvider = provider.GetRequiredService<IManageSecrets>();
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
            return connectionString;
        }

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
                    connectionString = GetConnectionString(configuration, provider, environment);

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
            services.AddScoped(typeof(DomainPersistentRepository<,>));

            //Dessert Repositories
            services.AddScoped<IDessertRepository, DessertRepository>();
            services.AddScoped<IDessertCategoryRepository, DessertCategoryRepository>();
            services.AddScoped<IDessertIngredientRepository, DessertIngredientRepository>();
            services.AddScoped<IInventoryAnalysisRepository, InventoryAnalysisRepository>();

            //Ingredient Repositories
            services.AddScoped<IIngredientRepository, IngredientRepository>();

            //MeasurementUnit Repositories
            services.AddScoped<IMeasurementUnitRepository, MeasurementUnitRepository>();

            //UnitConversion Repositories
            services.AddScoped<IUnitConversionRepository, UnitConversionRepository>();

            //PurchaseOrder Repositories
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IPendingReplenishmentRepository, PendingReplenishmentRepository>();

            //UnitOfWork services
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        //Add external services instead databases (for example, Mailjet and Azure Key Vault)
        public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration, string environment)
        {
            //Email sender services
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddTransient<IEmailRequestBuilder<MailjetRequest>, MailjetEmailRequestBuilder>();
            services.AddSingleton<IMailjetClientFactory<MailjetClient, MailjetResponse, MailjetRequest>, MailjetClientFactory>();
            services.AddScoped<IEmailSenderUrl<AppUser>, EmailConfirmationService>();
            services.AddScoped<IEmailSenderUrl<AppUser>, ResetPasswordService>();

            //Add external key vault services (implemented Azure Key Vault)
            services.AddTransient<IManageSecrets, ManageSecrets>();

            //Add external services to work with massive data (Import data from Excel)
            services.AddScoped<IImportData<IngredientUnitImportDto>, ImportIngredient>();
            services.AddScoped<IImportData<DessertAnalysis>, ImportMonthlyDessertDemand>();

            //Add Hangfire services
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                      .UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UseSqlServerStorage(
                        GetConnectionString(configuration, services.BuildServiceProvider(), environment)
                      )
            );

            //Add Background Job services
            //services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();

            services.AddHangfireServer();

            return services;
        }

        public static IServiceCollection AddConfigurationServices(this IServiceCollection services, IConfigurationManager configuration)
        {
            //Add customized configurations
            services.AddTransient<IConfigurationFactory<IConfiguration>, ConfigurationFactory>();

            //Add cache services
            services.AddMemoryCache();
            services.AddTransient<IDessertCategoryCacheService, DessertCategoryCacheService>();

            //Add Rate Limiting Services
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            //Add some default strategies to rate limiting
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

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
