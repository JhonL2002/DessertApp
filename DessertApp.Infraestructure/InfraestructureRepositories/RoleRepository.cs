using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Services.Infraestructure.RepositoriesServices.IdentityRepositories;
using DessertApp.Services.Infraestructure.RoleStoreServices;
using Microsoft.AspNetCore.Identity;

namespace DessertApp.Infraestructure.Repositories
{
    public class RoleRepository : IGenericIdentityRepository<AppRole, IdentityResult, string>
    {
        private readonly IRoleStore<AppRole> _roleStore;
        private readonly IExtendedRoleStore<AppRole> _extendedRoleStore;
        public RoleRepository(IRoleStore<AppRole> roleStore, IExtendedRoleStore<AppRole> extendedRoleStore)
        {
            _roleStore = roleStore;
            _extendedRoleStore = extendedRoleStore;
        }
        public async Task<IdentityResult> CreateAsync(AppRole role, CancellationToken cancellationToken)
        {
            await _roleStore.SetRoleNameAsync(role, role.Name, cancellationToken);
            await _roleStore.SetNormalizedRoleNameAsync(role, role.Name!.ToUpperInvariant(), cancellationToken);
            await _extendedRoleStore.SetConcurrencyStampAsync(role, Guid.NewGuid().ToString(), cancellationToken);
            var result = await _roleStore.CreateAsync(role, cancellationToken);
            return result;
        }

        public async Task<IdentityResult> DeleteAsync(AppRole entity, CancellationToken cancellationToken)
        {
            return await _roleStore.DeleteAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<AppRole>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _extendedRoleStore.GetAllRolesAsync(cancellationToken);
        }

        public async Task<AppRole> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (id == null) throw new ArgumentNullException(nameof(id), "Role does not exist!");
            return await _roleStore.FindByIdAsync(id, cancellationToken);
        }

        public async Task<AppRole> GetDetailsAsync(string id, CancellationToken cancellationToken)
        {
            return await _extendedRoleStore.GetRoleDetailsAsync(id, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(AppRole entity, CancellationToken cancellationToken)
        {
            return await _roleStore.UpdateAsync(entity, cancellationToken);
        }
    }
}
