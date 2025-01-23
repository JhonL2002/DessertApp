using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using DessertApp.Services.UnitOfWorkServices;

namespace DessertApp.Application.IngredientServices
{

    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IngredientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient, IngredientUnit ingredientUnit, CancellationToken cancellationToken)
        {
            ingredient.IsAvailable = ingredient.Stock > 0;
            ingredientUnit.Unit ??= await _unitOfWork.MeasurementUnits.GetByIdAsync(1, cancellationToken);
            ingredientUnit.Ingredient = ingredient;

            //Create ingredient
            var createdIngredient = await _unitOfWork.Ingredients.CreateAsync(ingredient, cancellationToken);
            
            //Assign ID into main ingredient for the IngredientUnit entity
            ingredientUnit.IngredientId = createdIngredient.Id;

            //Create initial unit from Ingredient
            await _unitOfWork.IngredientUnits.CreateAsync(ingredientUnit, cancellationToken);

            //Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return createdIngredient;
        }

        public async Task<bool> DeleteIngredientWhitUnitsAsync(int id, CancellationToken cancellationToken)
        {
            //Get the Ingredient with IngredientUnits
            var ingredient = await _unitOfWork.Ingredients.GetByIdWithDetailsAsync(
                id,
                cancellationToken,
                ingredient => ingredient.IngredientUnit!
            );

            if (ingredient == null) return false;

            //Convert IngredientUnits into array to push it at DeleteAsync method
            var ingredientUnit = ingredient.IngredientUnit;

            //Delete Ingredient with IngredientUnits
            await _unitOfWork.Ingredients.DeleteAsync(ingredient, cancellationToken, ingredientUnit!);

            //Commit changes in database
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.Ingredients.GetAllAsync(cancellationToken);
        }

        public async Task<Ingredient> GetIngredientByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Ingredients.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IngredientUnit> GetIngredientUnitByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _unitOfWork.IngredientUnits.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Ingredient> GetIngredientWithUnitsAsync(int id, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetIngredientWithUnitsAsync(id, cancellationToken);
        }

        public async Task<Ingredient> UpdateIngredientAsync(Ingredient ingredient, CancellationToken cancellationToken, IngredientUnit updatedUnit)
        {
            //Get existing ingredient with their units
            var existingIngredient = await _unitOfWork.Ingredients.GetByIdWithDetailsAsync(
                ingredient.Id,
                cancellationToken,
                i => i.IngredientUnit!
            ) ?? throw new Exception("Ingredient not found");

            //Update main ingredient
            existingIngredient.Name = ingredient.Name;
            existingIngredient.Stock = ingredient.Stock;
            existingIngredient.IsAvailable = ingredient.Stock > 0;

            //Update Ingredient Unit

            var existingIngredientUnit = existingIngredient.IngredientUnit;
            existingIngredientUnit!.ItemsPerUnit = updatedUnit.ItemsPerUnit;
            existingIngredientUnit.CostPerUnit = updatedUnit.CostPerUnit;
            existingIngredientUnit.OrderingCost = updatedUnit.OrderingCost;
            existingIngredientUnit.MonthlyHoldingCostRate = updatedUnit.MonthlyHoldingCostRate;
            existingIngredientUnit.AnnualDemand = updatedUnit.AnnualDemand;


            await _unitOfWork.IngredientUnits.UpdateAsync(existingIngredientUnit, cancellationToken);

            //Save changes in database
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return existingIngredient;
        }
    }
}
