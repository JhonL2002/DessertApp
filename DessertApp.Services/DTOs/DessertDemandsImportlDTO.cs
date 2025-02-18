using DessertApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace DessertApp.Services.DTOs
{
    public class DessertDemandsImportlDTO
    {
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int UnitId { get; set; }
        public MeasurementUnit Unit { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Range(1, double.MaxValue)]
        public decimal? UnitCost { get; set; }

        public int Month {  get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal? OrderingCost { get; set; }

        [Range(0.0, 1.0)]
        public decimal? MonthlyHoldingCostRate { get; set; }
    }
}
