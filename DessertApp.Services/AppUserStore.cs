using DessertApp.Models;
using DessertApp.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Services
{
    public class AppUserStore : IUserStore<AppUser>, IUserEmailStore<AppUser>, IUserPasswordStore<AppUser>
    {
        private readonly AppDbContext _context;
        //private bool _disposed = false;

        public AppUserStore(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Agrega el usuario al contexto
            _context.Users.Add(user);

            // Guarda los cambios en la base de datos
            var result = await _context.SaveChangesAsync(cancellationToken);

            // Devuelve un resultado exitoso si el usuario fue agregado correctamente
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to create user." });
        }

        public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Elimina el usuario del contexto
            _context.Users.Remove(user);

            // Guarda los cambios en la base de datos
            var result = await _context.SaveChangesAsync(cancellationToken);

            // Devuelve un resultado exitoso si el usuario fue eliminado correctamente
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

        public Task<AppUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<AppUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(normalizedUserName)) throw new ArgumentNullException(nameof(normalizedUserName));
            return await _context.Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        public Task<string?> GetEmailAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Devuelve si el correo del usuario está confirmado
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string?> GetNormalizedEmailAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Devuelve el correo electrónico normalizado
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string?> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Devuelve el nombre de usuario normalizado
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Devuelve el ID del usuario
            return Task.FromResult(user.Id);
        }

        public Task<string?> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task SetEmailAsync(AppUser user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(AppUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(AppUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(AppUser user, string? userName, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (userName == null) throw new ArgumentNullException(nameof(userName));

            user.UserName = userName;

            // Si estás trabajando con Entity Framework y deseas persistir los cambios en la base de datos
            //_context.Users.Update(user);
            //await _context.SaveChangesAsync(cancellationToken);
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            try
            {
                
                // Marcar el usuario como modificado
                _context.Users.Update(user);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync(cancellationToken);

                // Devolver un resultado exitoso
                return IdentityResult.Success;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejo de la excepción de concurrencia
                return IdentityResult.Failed(new IdentityError { Description = "El usuario fue modificado por otra operación." });
            }
            catch (Exception ex)
            {
                // Manejo de otros errores
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public Task SetPasswordHashAsync(AppUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash != null);
        }
    }
}
