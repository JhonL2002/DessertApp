using DessertApp.Models.IdentityModels;
using DessertApp.Services.Repositories;
using DessertApp.Services.RoleStoreServices;
using Microsoft.AspNetCore.Identity;

namespace DessertApp.Infraestructure.Repositories
{
    public class RoleRepository : IGenericRepository<IAppRole, IdentityResult, string>
    {
        private readonly IRoleStore<IAppRole> _roleStore;
        private readonly IExtendedRoleStore<IAppRole> _extendedRoleStore;
        public RoleRepository(IRoleStore<IAppRole> roleStore, IExtendedRoleStore<IAppRole> extendedRoleStore)
        {
            _roleStore = roleStore;
            _extendedRoleStore = extendedRoleStore;
        }
        public async Task<IdentityResult> CreateAsync(IAppRole entity, CancellationToken cancellationToken)
        {
            await _roleStore.SetRoleNameAsync(entity, entity.Name, cancellationToken);
            await _roleStore.SetNormalizedRoleNameAsync(entity, entity.Name!.ToUpperInvariant(), cancellationToken);
            await _extendedRoleStore.SetConcurrencyStampAsync(entity, Guid.NewGuid().ToString(), cancellationToken);
            return await _roleStore.CreateAsync(entity, cancellationToken);
        }

        public async Task<IdentityResult> DeleteAsync(IAppRole entity, CancellationToken cancellationToken)
        {
            return await _roleStore.DeleteAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<IAppRole>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _extendedRoleStore.GetAllRolesAsync(cancellationToken);
        }

        public async Task<IAppRole> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (id == null) throw new ArgumentNullException(nameof(id), "Role does not exist!");
            return await _roleStore.FindByIdAsync(id, cancellationToken);
        }

        public async Task<IAppRole> GetDetailsAsync(string id, CancellationToken cancellationToken)
        {
            return await _extendedRoleStore.GetRoleDetailsAsync(id, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(IAppRole entity, CancellationToken cancellationToken)
        {
            return await _roleStore.UpdateAsync(entity, cancellationToken);
        }
    }
}
