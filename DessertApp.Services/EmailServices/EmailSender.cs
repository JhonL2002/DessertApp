using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.EmailServices
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Sending email to {email} with subject {subject} ,Content: {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
