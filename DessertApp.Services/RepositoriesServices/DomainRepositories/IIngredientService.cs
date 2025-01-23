using DessertApp.Models.Entities;

namespace DessertApp.Services.RepositoriesServices.DomainRepositories
{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync(CancellationToken cancellationToken);
        Task<Ingredient> GetIngredientByIdAsync(int id, CancellationToken cancellationToken);
        Task<IngredientUnit> GetIngredientUnitByIdAsync(int id, CancellationToken cancellationToken);
        Task<Ingredient> GetIngredientWithUnitsAsync(int id, CancellationToken cancellationToken);
        Task<Ingredient> CreateIngredientAsync(Ingredient ingredient, IngredientUnit ingredientUnit, CancellationToken cancellationToken);
        Task<Ingredient> UpdateIngredientAsync(Ingredient ingredient, CancellationToken cancellationToken, IngredientUnit updatedUnit);
        Task<bool> DeleteIngredientWhitUnitsAsync(int id, CancellationToken cancellationToken);
    }
}
