using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices;
using DessertApp.Services.RepositoriesServices.DomainRepositories;

namespace DessertApp.Application.DessertServices
{
    public class DessertCategoryService : IDessertCategoryService
    {
        private readonly IDomainGenericRepository<DessertCategory, int> _dessertCategoryRepository;

        public DessertCategoryService(IDomainGenericRepository<DessertCategory, int> dessertCategoryRepository)
        {
            _dessertCategoryRepository = dessertCategoryRepository;
        }

        public async Task<DessertCategory> CreateDessertCategoryAsync(DessertCategory dessertCategory, CancellationToken cancellationToken)
        {
            await _dessertCategoryRepository.CreateAsync(dessertCategory, cancellationToken);
            return dessertCategory;
        }

        public async Task<bool> DeleteDessertCategoryAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _dessertCategoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null) return false;

            await _dessertCategoryRepository.DeleteAsync(category, cancellationToken);
            return true;
        }

        public async Task<IEnumerable<DessertCategory>> GetAllDessertCategoriesAsync(CancellationToken cancellationToken)
        {
            return await _dessertCategoryRepository.GetAllAsync(cancellationToken);
        }

        public async Task<DessertCategory?> GetDessertCategoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dessertCategoryRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<DessertCategory> UpdateDessertCategoryAsync(DessertCategory dessertCategory, CancellationToken cancellationToken)
        {
            await _dessertCategoryRepository.UpdateAsync(dessertCategory, cancellationToken);
            return dessertCategory;
        }
    }
}
