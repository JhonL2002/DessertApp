
using DessertApp.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace DessertApp.Infraestructure.IdentityModels
{
    public class AppUser : IdentityUser , IAppUser
    {
        //Add different properties here
    }
}
