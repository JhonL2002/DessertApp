using Microsoft.AspNetCore.Mvc;

namespace DessertApp.ViewModels.AccountVM
{
    public class ConfirmEmailVM
    {
        [TempData]
        public string StatusMessage { get; set; }
    }
}
