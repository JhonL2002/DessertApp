using DessertApp.Infraestructure.Data;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.EntityRepositories
{
    public class DessertRepository : IDessertRepository
    {
        private readonly DomainPersistentRepository<Dessert, int> _dessertRepository;

        public DessertRepository(DomainPersistentRepository<Dessert, int> dessertRepository)
        {
            _dessertRepository = dessertRepository;
        }

        public async Task<Dessert> CreateDessertAsync(Dessert dessert, CancellationToken cancellationToken)
        {
            await _dessertRepository.AddAsync(dessert, cancellationToken);
            await _dessertRepository.SaveChangesAsync(cancellationToken);
            return dessert;
        }

        public async Task<bool> DeleteDessertAsync(int id, CancellationToken cancellationToken)
        {
            var dessert = await _dessertRepository.GetByIdAsync(id, cancellationToken);
            if (dessert == null) return false;

            await _dessertRepository.DeleteAsync(dessert, cancellationToken);
            await _dessertRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<Dessert>> GetAllDessertsAsync(CancellationToken cancellationToken)
        {
            return await _dessertRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Dessert?> GetDessertByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dessertRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Dessert>> GetDessertsWithIngredientsAsync(CancellationToken cancellationToken)
        {
             return await _dessertRepository.GetAllWithDetailsAsync(
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
            return await _dessertRepository.GetByIdAsync(id, cancellationToken, query => query.Include(d => d.DessertCategory));
        }

        public async Task<Dessert> UpdateDessertAsync(Dessert dessert, CancellationToken cancellationToken)
        {
            await _dessertRepository.UpdateAsync(dessert, cancellationToken);
            await _dessertRepository.SaveChangesAsync(cancellationToken);
            return dessert;
        }
    }


}
