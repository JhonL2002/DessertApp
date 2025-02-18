using DessertApp.Models.Entities;
using DessertApp.Services.DTOs;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Ingredient>> GetAllIngredientsWithDetailsAsync(CancellationToken cancellationToken);
        Task<Ingredient?> GetIngredientByIdAsync(int id, CancellationToken cancellationToken);
        Task<Ingredient?> GetIngredientWithUnitsAsync(int id, CancellationToken cancellationToken);
        Task<Ingredient> CreateIngredientAsync(Ingredient ingredient, IngredientUnit ingredientUnit, CancellationToken cancellationToken);
        Task<List<IngredientUnitImportDto>> ImportIngredientsFromExternalSourceAsync(Stream source, CancellationToken cancellationToken);
        Task<List<Ingredient>> CreateIngredientsFromExternalSources(List<IngredientUnitImportDto> ingredientDtos, CancellationToken cancellationToken);
        Task<Ingredient> UpdateIngredientAsync(Ingredient ingredient, IngredientUnit updatedUnit, CancellationToken cancellationToken);
        Task<bool> DeleteIngredientWhitUnitsAsync(int id, CancellationToken cancellationToken);
    }
}
