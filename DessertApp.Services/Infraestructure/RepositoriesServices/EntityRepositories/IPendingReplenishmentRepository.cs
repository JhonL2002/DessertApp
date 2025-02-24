using DessertApp.Models.Entities;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories
{
    public interface IPendingReplenishmentRepository
    {
        Task AddPendingReplenishmentAsync(PendingReplenishment pendingReplenishment, CancellationToken cancellationToken);
        Task<PendingReplenishment?> GetNextPendingReplenishmentAsync(CancellationToken cancellationToken);
        Task RemovePendingReplenishmentAsync(PendingReplenishment pendingReplenishment, CancellationToken cancellationToken);
    }
}
