using DessertApp.Infraestructure.ConfigurationServices;
using DessertApp.Infraestructure.Data;
using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Infraestructure.UserServices
{
    public class AppUserStore : IUserStore<IAppUser>, IUserEmailStore<IAppUser>, IUserPasswordStore<IAppUser>, IUserRoleStore<IAppUser>
    {
        private readonly AppDbContext _context;

        public AppUserStore(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IdentityResult> CreateAsync(IAppUser user, CancellationToken cancellationToken)
        {
            /*if (user is not AppUser appUser)
            {
                throw new InvalidCastException("The provider user is not a type AppUser");
            }*/
            var appUser = VerifyCastingEntity<IAppUser, AppUser>.VerifyObject(user);
            _context.Users.Add(appUser);
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to create user." });
        }

        public async Task<IdentityResult> DeleteAsync(IAppUser user, CancellationToken cancellationToken)
        {
            var appUser = VerifyCastingEntity<IAppUser, AppUser>.VerifyObject(user);
            _context.Users.Remove(appUser);
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to delete user." });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public async Task<IAppUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _context.Users
               .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
        }

        public async Task<IAppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<IAppUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(normalizedUserName)) throw new ArgumentNullException(nameof(normalizedUserName));
            return await _context.Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        public Task<string?> GetEmailAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string?> GetNormalizedEmailAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string?> GetNormalizedUserNameAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id);
        }

        public Task<string?> GetUserNameAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task SetEmailAsync(IAppUser user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(IAppUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(IAppUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(IAppUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(IAppUser user, string? userName, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (userName == null) throw new ArgumentNullException(nameof(userName));

            user.UserName = userName;

            //_context.Users.Update(user);
            //await _context.SaveChangesAsync(cancellationToken);
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IAppUser user, CancellationToken cancellationToken)
        {
            var appUser = VerifyCastingEntity<IAppUser, AppUser>.VerifyObject(user);

            try
            {
                _context.Users.Update(appUser);

                await _context.SaveChangesAsync(cancellationToken);

                return IdentityResult.Success;
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(new IdentityError { Description = "The user was modified by another operation." });
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public Task SetPasswordHashAsync(IAppUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task AddToRoleAsync(IAppUser user, string roleName, CancellationToken cancellationToken)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == roleName, cancellationToken);
            if (role == null) throw new InvalidOperationException("Role not found.");

            var userRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            await _context.UserRoles.AddAsync(userRole, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveFromRoleAsync(IAppUser user, string roleName, CancellationToken cancellationToken)
        {
            var userRole = await _context.UserRoles
            .SingleOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == roleName, cancellationToken);

            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IList<string>> GetRolesAsync(IAppUser user, CancellationToken cancellationToken)
        {
            var roles =  await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => r.Name)
                .ToListAsync(cancellationToken);

            return roles!;
        }

        public async Task<bool> IsInRoleAsync(IAppUser user, string roleName, CancellationToken cancellationToken)
        {
            return await _context.UserRoles
                .Join(_context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => new { ur, r })
                .AnyAsync(userRole => userRole.ur.UserId == user.Id && userRole.r.Name == roleName, cancellationToken);
        }

        public async Task<IList<IAppUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return (IList<IAppUser>)await _context.UserRoles
                .Join(_context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => new { ur, r })
                .Where(ur => ur.r.Name == roleName)
                .Select(userRole => userRole.ur.UserId)
                .ToListAsync(cancellationToken);
        }
    }
}
