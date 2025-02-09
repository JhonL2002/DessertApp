using DessertApp.Infraestructure.Data;
using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.DomainRepositories
{
    public class DessertCategoryRepository : IDessertCategoryRepository
    {
        private readonly DomainPersistentRepository<DessertCategory, int> _dessertCategoryRepository;

        public DessertCategoryRepository(DomainPersistentRepository<DessertCategory, int> dessertCategoryRepository)
        {
            _dessertCategoryRepository = dessertCategoryRepository;
        }

        public async Task<DessertCategory> CreateDessertCategoryAsync(DessertCategory dessertCategory, CancellationToken cancellationToken)
        {
            await _dessertCategoryRepository.AddAsync(dessertCategory, cancellationToken);
            await _dessertCategoryRepository.SaveChangesAsync(cancellationToken);
            return dessertCategory;
        }

        public async Task<bool> DeleteDessertCategoryAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _dessertCategoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null) return false;

            await _dessertCategoryRepository.DeleteAsync(category, cancellationToken);
            await _dessertCategoryRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<DessertCategory>> GetAllDessertCategoriesAsync(CancellationToken cancellationToken)
        {
            return await _dessertCategoryRepository.GetAllAsync(cancellationToken);
        }

        public  async Task<IEnumerable<DessertCategory>> GetAllWithDessertsAsync(CancellationToken cancellationToken)
        {
            return await _dessertCategoryRepository.GetAllWithDetailsAsync(
                filter: null,
                cancellationToken: cancellationToken,
                include: query => query.Include(dc => dc.Desserts)
                );
        }

        public async Task<DessertCategory?> GetDessertCategoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dessertCategoryRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<DessertCategory> UpdateDessertCategoryAsync(DessertCategory dessertCategory, CancellationToken cancellationToken)
        {
            await _dessertCategoryRepository.UpdateAsync(dessertCategory, cancellationToken);
            await _dessertCategoryRepository.SaveChangesAsync(cancellationToken);
            return dessertCategory;
        }
    }
}
