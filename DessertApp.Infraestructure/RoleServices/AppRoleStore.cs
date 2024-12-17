using DessertApp.Infraestructure.ConfigurationServices;
using DessertApp.Infraestructure.Data;
using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Models.IdentityModels;
using DessertApp.Services.RoleStoreServices;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Infraestructure.RoleServices
{
    public class AppRoleStore : IExtendedRoleStore<IAppRole>, IRoleStore<IAppRole>
    {
        private readonly AppDbContext _context;
        //private bool _disposed = false;
        public AppRoleStore(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IdentityResult> CreateAsync(IAppRole role, CancellationToken cancellationToken)
        {
            var appRole = VerifyCastingEntity<IAppRole, AppRole>.VerifyObject(role);
            _context.Roles.Add(appRole);
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IAppRole role, CancellationToken cancellationToken)
        {
            var appRole = VerifyCastingEntity<IAppRole, AppRole>.VerifyObject(role);
            _context.Roles.Remove(appRole);
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
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

        public async Task<IAppRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _context.Roles.FindAsync([roleId], cancellationToken);
        }

        public async Task<IAppRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        }

        public Task<string?> GetNormalizedRoleNameAsync(IAppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(IAppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string?> GetRoleNameAsync(IAppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(IAppRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(IAppRole role, string? roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IAppRole role, CancellationToken cancellationToken)
        {
            var appRole = VerifyCastingEntity<IAppRole, AppRole>.VerifyObject(role);
            _context.Roles.Update(appRole);
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IEnumerable<IAppRole>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            return await _context.Roles.ToListAsync(cancellationToken);
        }

        public Task SetConcurrencyStampAsync(IAppRole role, string? concurrencyStamp, CancellationToken cancellationToken)
        {
            role.ConcurrencyStamp = concurrencyStamp;
            return Task.CompletedTask;
        }

        public async Task<IAppRole> GetRoleDetailsAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
