using DessertApp.Models.Entities;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories
{
    public interface IDessertRepository
    {
        Task<IEnumerable<Dessert>> GetDessertsWithIngredientsAsync(CancellationToken cancellationToken);
        Task<Dessert?> GetDessertWithCategoryAsync(int id, CancellationToken cancellationToken);
    }
}
