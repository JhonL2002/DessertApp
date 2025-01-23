using DessertApp.Application.ApplicationServicesInjectors;
using DessertApp.Infraestructure.ConfigurationServices;
using DessertApp.Infraestructure.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Configure the global culture (en-US)
CultureConfigurator.ConfigureCulture();

//Add configuration sources based on environment
var environment = builder.Environment.EnvironmentName;

if (environment == "Development")
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(443, listenOptions =>
        {
            listenOptions.UseHttps();
        });
        options.ListenAnyIP(8080);
    });

    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddUserSecrets<AppDbContext>();
}

//Configure Serilog
var logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

//Add extended services from Infraestructure layer
builder.Services.AddConfigurationServices();
builder.Services.AddExternalServices();
builder.Services.AddIdentityServices();
builder.Services.AddDatabaseServices(
    builder.Configuration,
    environment
);
builder.Services.AddRepositoriesServices();

//Add extended services from Application layer
builder.Services.AddApplicationServices();


//Add Serilog as global logger
builder.Logging.AddSerilog(logger);

// Add general services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

//Call datainitializer services when application starts
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await builder.Services.InitializeApplicationDataAsync(
    builder.Configuration,
    environment,
    services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
