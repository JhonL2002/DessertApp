using DessertApp.Models.Entities;

namespace DessertApp.Services.Application.Strategies
{
    public interface IUnitConversionStrategy<T,TSource>
    {
        List<T> ConvertUnits(List<TSource> sources, List<UnitConversion> conversions);
        List<T> ConvertToLongUnits(List<TSource> sources, List<UnitConversion> conversions);
    }
}
