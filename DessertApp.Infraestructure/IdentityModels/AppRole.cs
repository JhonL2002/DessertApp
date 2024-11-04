using DessertApp.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.IdentityModels
{
    public class AppRole : IdentityRole, IAppRole
    {
        public string Description { get; set; }
    }
}
