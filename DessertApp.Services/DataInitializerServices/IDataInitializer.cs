using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.DataInitializerServices
{
    public interface IDataInitializer
    {
        Task InitializeRolesAsync();
        Task InitializeAdminUserAsync(string adminEmail);
    }
}
