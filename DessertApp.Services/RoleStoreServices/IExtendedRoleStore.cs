
namespace DessertApp.Services.RoleStoreServices
{
    /// <summary>
    /// Extends <see cref="IRoleStore{TRole}"/> interface to include additional operations about role manager,
    /// as get all available roles from system.
    /// </summary>
    /// <typeparam name="TRole">The entity role type</typeparam>
    public interface IExtendedRoleStore<TRole>
    {
        /// <summary>
        /// Get a list of all available roles asynchronously
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to watch while waiting for the task to complete</param>
        /// <returns>A task that represents the asynchronous operation, containing an enumeration of all roles</returns>
        Task<IEnumerable<TRole>> GetAllRolesAsync(CancellationToken cancellationToken);
        Task<TRole> GetRoleDetailsAsync(string id, CancellationToken cancellationToken);
        Task SetConcurrencyStampAsync(TRole role, string? concurrencyStamp, CancellationToken cancellationToken);
    }
}
