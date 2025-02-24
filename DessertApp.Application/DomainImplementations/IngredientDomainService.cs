using DessertApp.Models.Entities;
using DessertApp.Services.Domain;

namespace DessertApp.Application.DomainImplementations
{
    /// <summary>
    /// Domain service that manage the replenishment of ingredients stock
    /// </summary>
    public class IngredientDomainService : IIngredientDomainService
    {
        public void ReplenishIngredientStock(Ingredient ingredient, int quantity)
        {
            if (ingredient == null)
            {
                throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null");
            }
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0");
            }

            ingredient.Stock = (ingredient.Stock ?? 0) + quantity;
        }
    }
}
