using System.ComponentModel.DataAnnotations;

namespace DessertApp.Services.DTOs
{
    public class IngredientUnitImportDto
    {
        public string IngredientName { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int? Stock { get; set; }

        public string UnitName { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int ItemsPerUnit { get; set; } = 0;

        [Range(0.0, double.MaxValue)]
        public decimal? CostPerUnit { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal? OrderingCost { get; set; }

        [Range(0.0, 1.0)]
        public decimal? MonthlyHoldingCostRate { get; set; }

        public int? AnnualDemand { get; set; }
    }
}
