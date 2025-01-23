using DessertApp.Models.Entities;

namespace DessertApp.Services.RepositoriesServices.DomainRepositories
{
    public interface IIngredientUnitService
    {
        Task<IngredientUnit> CreateIngredientUnitAsync(IngredientUnit ingredientUnit, int Id, CancellationToken cancellationToken);
    }
}
