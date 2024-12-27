using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DessertApp.Infraestructure.IdentityModels
{
    public class AppRole : IdentityRole
    {
        [Display(Name = "Normalized Name")]
        public override string? NormalizedName { get => base.NormalizedName; set => base.NormalizedName = value; }

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
