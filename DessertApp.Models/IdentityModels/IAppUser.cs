﻿
namespace DessertApp.Models.IdentityModels
{
    /// <summary>
    /// Defines a contract for application users in the system.
    /// This interface represents the basic properties that all users must
    /// have, including a description of the role.
    /// </summary>
    public interface IAppUser
    {
        //Properties from Microsoft.AspNetCore.Identity
        //Add some fileds if is necessary
        string Id { get; set; }
        string? UserName { get; set; }
        string? NormalizedUserName { get; set; }

        string? Email { get; set; }
        string? NormalizedEmail { get; set; }
        bool EmailConfirmed { get; set; }
        string? PasswordHash { get; set; }
        string? SecurityStamp { get; set; }
        string? ConcurrencyStamp { get; set; }
        string? PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        bool TwoFactorEnabled { get; set; }
        DateTimeOffset? LockoutEnd { get; set; }
        bool LockoutEnabled { get; set; }
        int AccessFailedCount { get; set; }
    }
}
