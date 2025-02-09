using DessertApp.Models.Entities;

namespace DessertApp.Services.RepositoriesServices.DomainRepositories
{
    public interface IDessertRepository
    {
        Task<Dessert> CreateDessertAsync(Dessert dessert, CancellationToken cancellationToken);
        Task<bool> DeleteDessertAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Dessert>> GetAllDessertsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Dessert>> GetDessertsWithIngredientsAsync(CancellationToken cancellationToken);
        Task<Dessert?> GetDessertByIdAsync(int id, CancellationToken cancellationToken);
        Task<Dessert?> GetDessertWithCategoryAsync(int id, CancellationToken cancellationToken);
        Task<Dessert> UpdateDessertAsync(Dessert dessert, CancellationToken cancellationToken);
    }
}
