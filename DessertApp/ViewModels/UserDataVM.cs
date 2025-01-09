using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DessertApp.ViewModels
{
    public class UserDataVM
    {
        public string? Username { get; set; }

        [TempData]
        public string? StatusMessage { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }
    }
}
