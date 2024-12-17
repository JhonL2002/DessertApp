using DessertApp.Infraestructure.ConfigurationServices;
using DessertApp.Infraestructure.Data;
using DessertApp.Services.DataInitializerServices;

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
builder.Services.AddInfraestructureServices(builder.Configuration, environment);
builder.Services.AddExternalServices();
builder.Services.AddApplicationServices();

// Add general services
builder.Services.AddControllersWithViews();

//Add identity services to project
/*builder.Services.AddIdentity<IAppUser, IAppRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();*/

builder.Services.AddRazorPages();


var app = builder.Build();

//Call SeedRolesAsync to create default roles in development scenario
using (var scope = app.Services.CreateScope())
{
    var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();

    //Acess directly to loaded configuration
    var adminEmail = builder.Configuration["AdminUserCredentials:AdminEmail"];
    var adminPass = builder.Configuration["AdminUserCredentials:AdminPass"];

    await dataInitializer.InitializeRolesAsync();
    await dataInitializer.InitializeAdminUserAsync(adminEmail!);
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
