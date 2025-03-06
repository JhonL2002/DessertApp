using DessertApp.Models.Entities;
using DessertApp.Services.DTOs;
using DessertApp.Services.Results;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Ingredient>> GetAllIngredientsWithDetailsAsync(CancellationToken cancellationToken);
        Task<Ingredient?> GetIngredientByIdAsync(int id, CancellationToken cancellationToken);
        Task<Ingredient?> GetIngredientWithUnitsAsync(int id, CancellationToken cancellationToken);
        Task<Ingredient> CreateIngredientAsync(Ingredient ingredient, IngredientUnit ingredientUnit, CancellationToken cancellationToken);
        Task<IngredientResult> ImportIngredientsFromExternalSourceAsync(Stream source, CancellationToken cancellationToken);
        Task<List<Ingredient>> CreateIngredientsFromExternalSources(List<IngredientUnitImportDto> ingredientDtos, CancellationToken cancellationToken);
        Task<Ingredient> UpdateIngredientWithUnitAsync(Ingredient ingredient, IngredientUnit updatedUnit, CancellationToken cancellationToken);
        Task<bool> DeleteIngredientWhitUnitsAsync(int id, CancellationToken cancellationToken);
    }
}
