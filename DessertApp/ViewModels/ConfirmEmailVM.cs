using Microsoft.AspNetCore.Mvc;

namespace DessertApp.ViewModels
{
    public class ConfirmEmailVM
    {
        [TempData]
        public string StatusMessage { get; set; }
    }
}
