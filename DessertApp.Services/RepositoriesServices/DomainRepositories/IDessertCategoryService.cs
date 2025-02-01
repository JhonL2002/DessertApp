using DessertApp.Models.Entities;

namespace DessertApp.Services.RepositoriesServices.DomainRepositories
{
    public interface IDessertCategoryService
    {
        Task<DessertCategory> CreateDessertCategoryAsync(DessertCategory dessertCategory, CancellationToken cancellationToken);
        Task<bool> DeleteDessertCategoryAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<DessertCategory>> GetAllDessertCategoriesAsync(CancellationToken cancellationToken);
        Task<DessertCategory?> GetDessertCategoryByIdAsync(int id, CancellationToken cancellationToken);
        Task<DessertCategory> UpdateDessertCategoryAsync(DessertCategory dessertCategory, CancellationToken cancellationToken);

    }
}
