using DessertApp.Models.Entities;
using DessertApp.Services.Application.ProductionServices;
using DessertApp.Services.Application.Strategies;
using DessertApp.Services.DTOs;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using Microsoft.Extensions.Logging;

namespace DessertApp.Application.ProductionServices
{
    public class ProductionService : IProductionService
    {
        private readonly IUnitConversionStrategy<DessertProductionDTO, Ingredient> _unitConversionStrategy;
        private readonly IDessertIngredientRepository _dessertIngredientRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IUnitConversionRepository _unitConversionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductionService> _logger;

        public ProductionService(IUnitConversionStrategy<DessertProductionDTO, Ingredient> unitConversionStrategy, IDessertIngredientRepository dessertIngredientRepository, IIngredientRepository ingredientRepository, IUnitConversionRepository unitConversionRepository, IUnitOfWork unitOfWork, ILogger<ProductionService> logger)
        {
            _unitConversionStrategy = unitConversionStrategy;
            _dessertIngredientRepository = dessertIngredientRepository;
            _ingredientRepository = ingredientRepository;
            _unitConversionRepository = unitConversionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> ProduceDessertsAsync(int dessertId, string month, CancellationToken cancellationToken)
        {
            List<Ingredient> ingredientsToUpdate = new List<Ingredient>();
            _logger.LogInformation("Starting Dessert Production for Id: {DessertId}", dessertId);

            var recipe = await _dessertIngredientRepository.GetDessertIngredientsByDessertIdAsync(dessertId, cancellationToken);

            if (recipe == null || !recipe.Any())
            {
                _logger.LogWarning("Recipe not found with Dessert Id {DessertId}", dessertId);
                return 0;
            }

            var stockIngredients = await _ingredientRepository.GetAllIngredientsWithDetailsAsync(cancellationToken);
            var conversions = await _unitConversionRepository.GetAllUnitConversion(cancellationToken);
            var convertedStock = _unitConversionStrategy.ConvertUnits(stockIngredients.ToList(), conversions.ToList());

            //Determine how much desserts can be produced
            var allDessertDemands = await _unitOfWork.DessertDemands
                .GetAllAsync(cancellationToken);

            var dessertDemand = allDessertDemands.FirstOrDefault(d => d.DessertId == dessertId && d.Month == month);

            int maxDesserts = dessertDemand?.Demand ?? 0;

            foreach (var recipeIngredient in recipe)
            {
                var stockIngredient = convertedStock.FirstOrDefault(i => i.IngredientId == recipeIngredient!.IngredientId);
                if (stockIngredient == null || stockIngredient.Quantity <= 0)
                {
                    _logger.LogWarning("Unavailable ingredient or not found in stock: {IngredientId}", recipeIngredient!.IngredientId);
                    return 0;
                }

                int possibleWithThisIngredient = (int)(stockIngredient.Quantity / recipeIngredient!.QuantityRequired);
                maxDesserts = Math.Min(maxDesserts, possibleWithThisIngredient);
                stockIngredient.Quantity -= recipeIngredient.QuantityRequired * maxDesserts;
                _logger.LogInformation("Reducing stock from ingredient {IngredientId}, new stock: {Stock}", stockIngredient.IngredientId, stockIngredient.Quantity);

                var updatedIngredient = await _unitOfWork.Ingredients.GetByIdAsync(stockIngredient.IngredientId, cancellationToken);
                updatedIngredient!.Stock = (int)stockIngredient.Quantity;
                ingredientsToUpdate.Add(updatedIngredient!);
            }

            var longConvertedStock = _unitConversionStrategy.ConvertToLongUnits(ingredientsToUpdate, conversions.ToList());

            foreach (var ingredient in longConvertedStock)
            {
                var existingIngredient = await _unitOfWork.Ingredients.GetByIdAsync(ingredient.IngredientId, cancellationToken);
                if (existingIngredient != null)
                {
                    existingIngredient.Stock = (int)ingredient.Quantity;
                    await _unitOfWork.Ingredients.UpdateAsync(existingIngredient, cancellationToken);
                }
            }

            var existingDessertStock = await _unitOfWork.Desserts.GetByIdAsync(dessertId, cancellationToken);
            if (existingDessertStock != null)
            {
                existingDessertStock.Stock += maxDesserts;
                await _unitOfWork.Desserts.UpdateAsync(existingDessertStock, cancellationToken);
                _logger.LogInformation("Stock updated for dessert {DessertId}, new stock: {Stock}", dessertId, existingDessertStock.Stock);
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Prtoduction completed. Produced desserts: {MaxDesserts}", maxDesserts);

            return maxDesserts;
        }
    }
}
