using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Models.IdentityModels;
using DessertApp.Services.DataInitializerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
namespace DessertApp.Infraestructure.DataInitializerServices
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IServiceProvider _serviceProvider;
        public DataInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task InitializeRolesAsync()
        {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            //Define some roles to use in app
            var roles = new List<AppRole>
            {
                new AppRole { Name = "Admin", Description = "Administrators with full permissions" },
                new AppRole { Name = "User", Description = "Regular users with limited permissions" }
            };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name!))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }

        public async Task InitializeAdminUserAsync(string adminEmail)
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                var result = await userManager.AddToRoleAsync(adminUser, "Admin");
                if (result.Succeeded)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("User assigned successfully to role!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to assign user to role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("User with added role already exists!");
            }
        }
    }
}
