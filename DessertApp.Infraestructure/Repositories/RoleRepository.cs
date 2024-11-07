using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Services.Repositories;
using DessertApp.Services.RoleStoreServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.Repositories
{
    public class RoleRepository : IGenericRepository<AppRole, IdentityResult, string>
    {
        private readonly IRoleStore<AppRole> _roleStore;
        private readonly IExtendedRoleStore<AppRole> _extendedRoleStore;
        public RoleRepository(IRoleStore<AppRole> roleStore, IExtendedRoleStore<AppRole> extendedRoleStore)
        {
            _roleStore = roleStore;
            _extendedRoleStore = extendedRoleStore;
        }
        public async Task<IdentityResult> CreateAsync(AppRole entity, CancellationToken cancellationToken)
        {
            await _roleStore.SetRoleNameAsync(entity, entity.Name, cancellationToken);
            await _roleStore.SetNormalizedRoleNameAsync(entity, entity.Name!.ToUpperInvariant(), cancellationToken);
            await _extendedRoleStore.SetConcurrencyStampAsync(entity, Guid.NewGuid().ToString(), cancellationToken);
            return await _roleStore.CreateAsync(entity, cancellationToken);
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
