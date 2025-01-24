using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.ViewModels.AccountVM
{
    public class RegisterConfirmationVM
    {
        public string Email { get; set; }
        public bool DisplayConfirmAccountLink { get; set; }
        public string? EmailConfirmationUrl { get; set; }
    }
}
