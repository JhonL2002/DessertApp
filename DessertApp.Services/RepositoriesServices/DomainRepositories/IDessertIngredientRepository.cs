using DessertApp.Models.Entities;

namespace DessertApp.Services.RepositoriesServices.DomainRepositories
{
    public interface IDessertIngredientRepository
    {
        Task CreateDessertIngredientAsync(List<DessertIngredient> dessertIngredients, CancellationToken cancellationToken);
        Task<bool> DeleteDessertIngredientAsync(int dessertId, CancellationToken cancellationToken);
        Task<IEnumerable<DessertIngredient>> GetAllDessertIngredientsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<DessertIngredient?>> GetDessertIngredientsByDessertIdAsync(int dessertId, CancellationToken cancellationToken);
        Task<DessertIngredient> UpdateDessertIngredientAsync(DessertIngredient dessertIngredient, CancellationToken cancellationToken); 
    }
}
