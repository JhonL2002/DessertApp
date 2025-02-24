using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Infraestructure.EntityRepositories
{
    public class InventoryAnalysisRepository : IInventoryAnalysisRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryAnalysisRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<InventoryAnalysis>> GetInventoryAnalysesWithDetailsAsync(CancellationToken cancellationToken)
        {
            return _unitOfWork.InventoryAnalysis.GetAllWithDetailsAsync(
                filter: null,
                cancellationToken: cancellationToken,
                query => query
                    .Include(ia => ia.Ingredient)
                        .ThenInclude(i => i.IngredientUnit));
        }
    }
}
