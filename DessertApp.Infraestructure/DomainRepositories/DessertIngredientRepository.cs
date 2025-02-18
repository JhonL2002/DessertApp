using DessertApp.Infraestructure.Data;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.DomainRepositories
{
    public class DessertIngredientRepository : IDessertIngredientRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DomainPersistentRepository<DessertIngredient, int> _dessertIngredientRepository;

        public DessertIngredientRepository(DomainPersistentRepository<DessertIngredient, int> dessertIngredientRepository, IUnitOfWork unitOfWork)
        {
            _dessertIngredientRepository = dessertIngredientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateDessertIngredientAsync(List<DessertIngredient> dessertIngredients, CancellationToken cancellationToken)
        {
            if (dessertIngredients == null || dessertIngredients.Count == 0)
                throw new ArgumentException("The ingredients list cannot be empty");

            await _unitOfWork.DessertIngredients.AddRangeAsync(dessertIngredients, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteDessertIngredientAsync(int dessertId, CancellationToken cancellationToken)
        {
            var dessertIngredients = await _dessertIngredientRepository.GetAllWithDetailsAsync(
                di => di.DessertId == dessertId,
                cancellationToken,
                query => query.Include(di => di.Dessert));

            if (dessertIngredients == null || !dessertIngredients.Any()) return false;

            await _dessertIngredientRepository.DeleteRangeAsync(dessertIngredients, cancellationToken);
            await _dessertIngredientRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<DessertIngredient>> GetAllDessertIngredientsAsync(CancellationToken cancellationToken)
        {
            var result = await _dessertIngredientRepository.GetAllWithDetailsAsync(
                null,
                cancellationToken,
                query => query.Include(di => di.Dessert));

            //Group by DessertId to avoid duplicated data
            var uniqueDesserts = result
                .GroupBy(di => di.DessertId)
                .Select(g => g.First()) //Take only first register by dessert
                .ToList();

            return uniqueDesserts;
        }

        public async Task<IEnumerable<DessertIngredient?>> GetDessertIngredientsByDessertIdAsync(int dessertId, CancellationToken cancellationToken)
        {
            return await _dessertIngredientRepository.GetAllWithDetailsAsync(
                di => di.DessertId == dessertId,
                cancellationToken,
                query => query.Include(di => di.Unit).Include(di => di.Ingredient)
            );
        }

        public async Task<DessertIngredient> UpdateDessertIngredientAsync(DessertIngredient dessertIngredient, CancellationToken cancellationToken)
        {
            await _dessertIngredientRepository.UpdateAsync(dessertIngredient, cancellationToken);
            await _dessertIngredientRepository.SaveChangesAsync(cancellationToken);
            return dessertIngredient;
        }
    }
}
