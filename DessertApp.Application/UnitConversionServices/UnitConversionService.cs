using DessertApp.Models.Entities;
using DessertApp.Services.Application.UnitConversionServices;
using DessertApp.Services.DTOs;

namespace DessertApp.Application.UnitConversionServices
{
    public class UnitConversionService : IUnitConversionService
    {
        public List<DessertDemandsImportlDTO> ConvertUnits(List<DessertDemandsImportlDTO> orderDetailsDTO, List<UnitConversion> conversions)
        {
            List<DessertDemandsImportlDTO> convertedDetails = new List<DessertDemandsImportlDTO>();

            foreach (var detail in orderDetailsDTO)
            {
                var conversion = conversions.FirstOrDefault(c => c.FromUnit.Name == detail.Unit.Name && c.ConversionFactor > 0);

                if (conversion != null)
                {
                    convertedDetails.Add(new DessertDemandsImportlDTO
                    {
                        IngredientId = detail.IngredientId,
                        Ingredient = detail.Ingredient,
                        UnitId = conversion.ToUnit.Id,
                        Unit = new MeasurementUnit
                        {
                            Id = conversion.ToUnit.Id,
                            Name = conversion.ToUnit.Name
                        },
                        Quantity = Math.Ceiling(detail.Quantity / conversion.ConversionFactor),
                        UnitCost = detail.UnitCost,
                        Month = detail.Month,
                    });
                }
                else
                {
                    convertedDetails.Add(detail);
                }
            }

            return convertedDetails;
        }
    }
}
