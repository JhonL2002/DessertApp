
using DessertApp.Models.Entities;

namespace DessertApp.Services.Domain
{
    public interface IIngredientDomainService
    {
        void ReplenishIngredientStock(Ingredient ingredient, int quantity);
    }
}
