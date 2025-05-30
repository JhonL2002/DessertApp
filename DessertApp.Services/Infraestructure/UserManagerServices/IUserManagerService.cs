﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.Infraestructure.UserManagerServices
{
    public interface IUserManagerService<TResult, T, TOptions> where T : class
    {
        Task<(TResult result, T user)> CreateUserAsync(string email, string password);
        Task<TResult> ConfirmEmailAsync(string userId, string code);
        Task<T?> FindByEmailAsync(string email);
        Task<T?> FindByIdAsync(string userId);
        Task<string> GenerateEmailConfirmationTokenAsync(T user);
        Task<string> GeneratePasswordResetTokenAsync(T user);
        Task<string> GetPhoneNumberAsync(T user);
        Task<TResult> SetPhoneNumberAsync(T user, string? phoneNumber);
        Task<T> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<bool> IsInRoleAsync(T user, string role);
        Task<TResult> ResetPasswordAsync(T user, string token, string newPassword);
        TOptions? Options { get; set; }
    }
}
