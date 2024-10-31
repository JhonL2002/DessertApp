using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Models.Data
{
    public static class RoleInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

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
    }
}
