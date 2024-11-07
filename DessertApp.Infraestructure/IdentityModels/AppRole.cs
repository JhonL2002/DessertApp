using DessertApp.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.IdentityModels
{
    public class AppRole : IdentityRole, IAppRole
    {
        [Display(Name = "Normalized Name")]
        public override string? NormalizedName { get => base.NormalizedName; set => base.NormalizedName = value; }
        public string Description { get; set; }
    }
}
