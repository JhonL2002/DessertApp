using DessertApp.Models;
using DessertApp.Models.Data;
using DessertApp.Services;
using DessertApp.Services.EmailServices;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLString"));
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
builder.Services.AddTransient<IUserStore<AppUser>, AppUserStore>();
builder.Services.AddTransient<IRoleStore<AppRole>, AppRoleStore>();

//Send Email services
builder.Services.AddTransient<IMailjetClient>(provider =>
{
    //Add the ApiKey and SecretKey from Mailjet
    var configuration = provider.GetRequiredService<IConfiguration>();
    //The ApiKey and SecretKey added in secrets.json of DessertApp Project
    return new MailjetClient(configuration["EmailCredentials:ApiKey"], configuration["EmailCredentials:SecretKey"]);
});
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IEmailRequestBuilder, MailjetEmailRequestBuilder>();


var app = builder.Build();

//Call SeedRolesAsync to create default roles
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await RoleInitializer.SeedRolesAsync(serviceProvider);
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
