using DessertApp.Models.Entities;
using DessertApp.Services.DTOs;
using DessertApp.Services.ImportDataServices;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using DessertApp.Services.UnitOfWorkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.DomainRepositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImportIngredient<IngredientUnitImportDto> _importIngredient;

        public IngredientRepository(IUnitOfWork unitOfWork, IImportIngredient<IngredientUnitImportDto> importIngredient)
        {
            _unitOfWork = unitOfWork;
            _importIngredient = importIngredient;
        }

        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient, IngredientUnit ingredientUnit, CancellationToken cancellationToken)
        {
            ingredient.IsAvailable = ingredient.Stock > 0;
            ingredientUnit.Unit ??= await _unitOfWork.MeasurementUnits.GetByIdAsync(4, cancellationToken);
            ingredientUnit.Ingredient = ingredient;

            //Create ingredient
            var createdIngredient = await _unitOfWork.Ingredients.AddAsync(ingredient, cancellationToken);

            //Assign ID into main ingredient for the IngredientUnit entity
            ingredientUnit.IngredientId = createdIngredient.Id;

            //Create initial unit from Ingredient
            await _unitOfWork.IngredientUnits.AddAsync(ingredientUnit, cancellationToken);

            //Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return createdIngredient;
        }

        public async Task<List<Ingredient>> CreateIngredientsFromExternalSources(List<IngredientUnitImportDto> ingredientDtos, CancellationToken cancellationToken)
        {
            var ingredients = new List<Ingredient>();

            //Process each DTO and insert all ingredients from file
            foreach (var dto in ingredientDtos)
            {
                //Verify if ingredient already exists
                var existingIngredient = await _unitOfWork.Ingredients.GetByFieldAsync("Name", dto.IngredientName, cancellationToken);
                if (existingIngredient != null)
                {
                    //If ingredient exists, don't attach into EF
                    _unitOfWork.Ingredients.Attach(existingIngredient, cancellationToken);

                    //If ingredient exists, add to list an continue
                    ingredients.Add(existingIngredient);
                    continue;
                }
                else
                {
                    //Create a new ingredient if not exists
                    var ingredient = new Ingredient
                    {
                        Name = dto.IngredientName,
                        Stock = dto.Stock ?? 0,
                        IsAvailable = (dto.Stock ?? 0) > 0
                    };

                    //Save new ingredient to generate Id
                    await _unitOfWork.Ingredients.AddAsync(ingredient, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    ingredients.Add(ingredient);

                    existingIngredient = ingredient;
                }
                //Get MeasurementUnits
                var measurementUnit = await _unitOfWork.MeasurementUnits.GetByFieldAsync("Name", dto.UnitName, cancellationToken) ?? throw new InvalidOperationException($"The measurement unit '{dto.UnitName}' does not exist");

                //Create and associate the ingredient with unit
                var ingredientUnit = new IngredientUnit
                {
                    IngredientId = existingIngredient.Id,
                    UnitId = measurementUnit!.Id,
                    ItemsPerUnit = dto.ItemsPerUnit,
                    CostPerUnit = dto.CostPerUnit,
                    OrderingCost = dto.OrderingCost,
                    MonthlyHoldingCostRate = dto.MonthlyHoldingCostRate,
                    AnnualDemand = dto.AnnualDemand,
                };

                //Register IngredientUnit
                await _unitOfWork.IngredientUnits.AddAsync(ingredientUnit, cancellationToken);
            }

            //Save all changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ingredients;
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

        public async Task<Ingredient?> GetIngredientByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Ingredients.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IngredientUnit?> GetIngredientUnitByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _unitOfWork.IngredientUnits.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Ingredient?> GetIngredientWithUnitsAsync(int id, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Ingredients.GetByIdWithDetailsAsync(
                id,
                cancellationToken,
                ingredient => ingredient.IngredientUnit!);
        }

        public async Task<List<IngredientUnitImportDto>> ImportIngredientsFromExternalSourceAsync(Stream source, CancellationToken cancellationToken)
        {
            return await _importIngredient.ImportFromExternalSourceAsync(source);
        }

        public async Task<Ingredient> UpdateIngredientAsync(Ingredient ingredient, IngredientUnit updatedUnit, CancellationToken cancellationToken)
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
