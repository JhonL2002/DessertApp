using DessertApp.Models.Entities;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories
{
    public interface IInventoryAnalysisRepository
    {
        Task<IEnumerable<InventoryAnalysis>> GetInventoryAnalysesWithDetailsAsync(CancellationToken cancellationToken);
    }
}
