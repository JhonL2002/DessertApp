using DessertApp.Models.Entities;
using DessertApp.Services.Application.Strategies;
using DessertApp.Services.DTOs;

namespace DessertApp.Application.UnitConversionServices
{
    public class SmallUnitConversionStrategy : IUnitConversionStrategy<DessertProductionDTO, Ingredient>
    {
        public List<DessertProductionDTO> ConvertToLongUnits(List<Ingredient> ingredients, List<UnitConversion> conversions)
        {
            List<DessertProductionDTO> convertedDetails = new List<DessertProductionDTO>();
            foreach (var ingredient in ingredients)
            {
                var longConversion = conversions.FirstOrDefault(
                    c => c.ToUnit.Name.Equals(ingredient.IngredientUnit!.Unit!.Name, StringComparison.OrdinalIgnoreCase) && c.ConversionFactor > 0);

                if (longConversion != null)
                {
                    convertedDetails.Add(new DessertProductionDTO
                    {
                        IngredientId = ingredient.Id,
                        Unit = new MeasurementUnit
                        {
                            Id = longConversion.FromUnit.Id,
                            Name = longConversion.FromUnit.Name
                        },
                        Quantity = ingredient.Stock / longConversion.ConversionFactor ?? 0
                    });
                }
            }

            return convertedDetails;
        }

        public List<DessertProductionDTO> ConvertUnits(List<Ingredient> ingredients, List<UnitConversion> conversions)
        {
            List<DessertProductionDTO> convertedDetails = new List<DessertProductionDTO>();

            foreach(var ingredient in ingredients)
            {
                var smallConversion = conversions.FirstOrDefault(
                    c => c.ToUnit.Name.Equals(ingredient.IngredientUnit!.Unit!.Name, StringComparison.OrdinalIgnoreCase) && c.ConversionFactor > 0);

                if (smallConversion != null)
                {
                    convertedDetails.Add(new DessertProductionDTO
                    {
                        IngredientId = ingredient.Id,
                        Unit = new MeasurementUnit
                        {
                            Id = smallConversion.ToUnit.Id,
                            Name = smallConversion.ToUnit.Name,
                        },
                        Quantity = ingredient.Stock * smallConversion.ConversionFactor ?? 0
                    });
                }
            }

            return convertedDetails;
        }
    }
}
