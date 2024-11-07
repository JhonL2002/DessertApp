using DessertApp.Infraestructure.ConfigurationServices;
using DessertApp.Infraestructure.Data;
using DessertApp.Infraestructure.DataInitializerServices;
using DessertApp.Infraestructure.EmailServices;
using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Infraestructure.Repositories;
using DessertApp.Infraestructure.RoleServices;
using DessertApp.Infraestructure.UserServices;
using DessertApp.Services.ConfigurationServices;
using DessertApp.Services.DataInitializerServices;
using DessertApp.Services.EmailServices;
using DessertApp.Services.IEmailServices;
using DessertApp.Services.Repositories;
using DessertApp.Services.RoleStoreServices;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add SQLServer database to project
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLString"),
        options => options.MigrationsAssembly("DessertApp.Infraestructure"));
});

//Add identity services to project
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages();

//Identity Services
builder.Services.AddScoped<IUserStore<AppUser>, AppUserStore>();
builder.Services.AddScoped<IExtendedRoleStore<AppRole>, AppRoleStore>();
builder.Services.AddScoped<IRoleStore<AppRole>, AppRoleStore>();

//Data Initializer Services
builder.Services.AddScoped<IDataInitializer, DataInitializer>();

//Send Email services
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IEmailRequestBuilder<MailjetRequest>, MailjetEmailRequestBuilder>();
builder.Services.AddSingleton<IMailjetClientFactory<MailjetClient>, MailjetClientFactory>();

//Customized Configuration Services to read secrets in the Infraestructure layer
builder.Services.AddTransient<IConfigurationFactory<IConfiguration>, ConfigurationFactory>();
builder.Services.AddSingleton<IConfigurationFactory<IConfiguration>, ConfigurationFactory>();

//Repositories services
builder.Services.AddScoped<IGenericRepository<AppRole, IdentityResult, string>, RoleRepository>();



var app = builder.Build();

//Call SeedRolesAsync to create default roles
using (var scope = app.Services.CreateScope())
{
    var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>(); 
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    await dataInitializer.InitializeRolesAsync();
    await dataInitializer.InitializeAdminUserAsync(configuration["AdminUserCredentials:AdminEmail"]!);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
