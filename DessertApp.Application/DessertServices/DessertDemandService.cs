using DessertApp.Models.Entities;
using DessertApp.Services.Application.DessertServices;
using DessertApp.Services.DTOs;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using System.ComponentModel.DataAnnotations;

namespace DessertApp.Application.DessertServices
{
    public class DessertDemandService : IDessertDemandService
    {
        private readonly IDessertRepository _dessertRepository;
        private readonly IUnitConversionRepository _unitConversionRepository;

        public DessertDemandService(IDessertRepository dessertRepository, IUnitConversionRepository unitConversionRepository)
        {
            _dessertRepository = dessertRepository;
            _unitConversionRepository = unitConversionRepository;
        }

        public async Task<List<DessertDemandsImportlDTO>> GetDessertDemands(List<DessertAnalysis> importedDessertAnalysis, CancellationToken cancellationToken)
        {
            List<DessertDemandsImportlDTO> values = new();

            //Create a dictionary for fast access to data
            var dessertAnalysisDict = importedDessertAnalysis.ToDictionary(d => d.DessertName);

            //Get al desserts with ingredients
            var desserts = await _dessertRepository.GetDessertsWithIngredientsAsync(cancellationToken) ?? [];

            //Get all unit conversions
            var conversions = await _unitConversionRepository.GetAllUnitConversion(cancellationToken);

            foreach (var dessert in desserts)
            {
                if (!dessertAnalysisDict.TryGetValue(dessert.Name, out var matchedDessert))
                    continue; //If dessert is not found, skip dessert

                values.AddRange(
                    matchedDessert.MonthlyDemands.SelectMany(item =>
                        dessert.DessertIngredients.Select(des =>
                        {
                            int monthlyDemand = (int)(des.QuantityRequired * item[1]);

                            //Apply unit conversion
                            var conversion = conversions.FirstOrDefault(c => c.FromUnitId == des.Unit.Id && c.ToUnitId == des.Ingredient.IngredientUnit?.UnitId);
                            var quantity = conversion != null ? des.QuantityRequired * item[1] / conversion.ConversionFactor : des.QuantityRequired * item[1];
                            return new DessertDemandsImportlDTO
                            {
                                IngredientId = des.IngredientId,
                                Ingredient = des.Ingredient,
                                Unit = conversion?.ToUnit ?? des.Unit,
                                Quantity = quantity,
                                UnitCost = des.Ingredient.IngredientUnit?.CostPerUnit ?? 0,
                                Month = item[0],
                                OrderingCost = des.Ingredient.IngredientUnit?.OrderingCost ?? 0,
                                MonthlyHoldingCostRate = des.Ingredient.IngredientUnit?.MonthlyHoldingCostRate ?? 0,
                            };
                        })
                    )
                );
            }

            return values;
        }
    }
}
