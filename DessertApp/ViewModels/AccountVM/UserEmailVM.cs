using System.ComponentModel.DataAnnotations;

namespace DessertApp.ViewModels.AccountVM
{
    public class UserEmailVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
