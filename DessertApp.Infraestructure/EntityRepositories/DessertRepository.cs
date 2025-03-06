using DessertApp.Infraestructure.Data;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Infraestructure.EntityRepositories
{
    public class DessertRepository : IDessertRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public DessertRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Dessert>> GetDessertsWithIngredientsAsync(CancellationToken cancellationToken)
        {
             return await _unitOfWork.Desserts.GetAllWithDetailsAsync(
                filter: null,
                cancellationToken: cancellationToken,
                include: query => query
                    .Include(d => d.DessertIngredients)
                        .ThenInclude(i => i.Ingredient)
                            .ThenInclude(i => i.IngredientUnit)
                    .Include(d => d.DessertIngredients)
                        .ThenInclude(di => di.Unit)
            );
        }

        public async Task<Dessert?> GetDessertWithCategoryAsync(int id, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Desserts.GetByIdAsync(id, cancellationToken, query => query.Include(d => d.DessertCategory));
        }
    }


}
