using DessertApp.Infraestructure.Data;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Infraestructure.DomainRepositories
{
    public class InventoryAnalysisRepository : IInventoryAnalysisRepository
    {
        private readonly DomainPersistentRepository<InventoryAnalysis, int> _inventoryAnalysisRepository;

        public InventoryAnalysisRepository(DomainPersistentRepository<InventoryAnalysis, int> inventoryAnalysisRepository)
        {
            _inventoryAnalysisRepository = inventoryAnalysisRepository;
        }

        public Task<IEnumerable<InventoryAnalysis>> GetInventoryAnalysesWithDetailsAsync(CancellationToken cancellationToken)
        {
            return _inventoryAnalysisRepository.GetAllWithDetailsAsync(
                filter: null,
                cancellationToken: cancellationToken,
                query => query
                    .Include(ia => ia.Ingredient)
                        .ThenInclude(i => i.IngredientUnit));
        }
    }
}
