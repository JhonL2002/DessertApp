using DessertApp.Infraestructure.ConfigurationServices;
using DessertApp.Infraestructure.Data;
using DessertApp.Services.DataInitializerServices;
using DessertApp.Services.SecretServices;

var builder = WebApplication.CreateBuilder(args);

//Add configuration sources based on environment
var environment = builder.Environment.EnvironmentName;

if (environment == "Development")
{
    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddUserSecrets<AppDbContext>();
}

//Add extended services from infraestructure layer
builder.Services.AddExternalServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfraestructureServices(
    builder.Configuration,
    environment
);

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
