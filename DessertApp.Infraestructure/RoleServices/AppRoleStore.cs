using DessertApp.Infraestructure.Data;
using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Models.IdentityModels;
using DessertApp.Services.RoleStoreServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Infraestructure.RoleServices
{
    public class AppRoleStore : IExtendedRoleStore<AppRole>, IRoleStore<AppRole>
    {
        private readonly AppDbContext _context;
        //private bool _disposed = false;
        public AppRoleStore(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IdentityResult> CreateAsync(AppRole role, CancellationToken cancellationToken)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AppRole role, CancellationToken cancellationToken)
        {
            _context.Roles.Remove(role);
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

        public async Task<AppRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _context.Roles.FindAsync([roleId], cancellationToken);
        }

        public async Task<AppRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        }

        public Task<string?> GetNormalizedRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string?> GetRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(AppRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(AppRole role, string? roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(AppRole role, CancellationToken cancellationToken)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IEnumerable<AppRole>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            return await _context.Roles.ToListAsync(cancellationToken);
        }

        public Task SetConcurrencyStampAsync(AppRole role, string? concurrencyStamp, CancellationToken cancellationToken)
        {
            role.ConcurrencyStamp = concurrencyStamp;
            return Task.CompletedTask;
        }

        public async Task<AppRole> GetRoleDetailsAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
