
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DessertApp.Models.Entities
{
    public class IngredientUnit
    {
        public int Id { get; set; }

        public int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }

        public int UnitId { get; set; }
        public MeasurementUnit? Unit { get; set; }

        //Optional additional details
        [Range(0, int.MaxValue)]
        public int ItemsPerUnit { get; set; } = 0; // Number of items in a box, bottle, package

        [Range(0.0, double.MaxValue)]
        public decimal? CostPerUnit { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal? OrderingCost { get; set; }

        [Range(0.0, 1.0)]
        public decimal? MonthlyHoldingCostRate { get; set; }

        [NotMapped]
        public int? AnnualDemand {  get; set; }

        [NotMapped]
        public decimal AnnualHoldingCost => CostPerUnit.HasValue && MonthlyHoldingCostRate.HasValue
            ? CostPerUnit.Value * MonthlyHoldingCostRate.Value * 12
            : 0;

        [NotMapped]
        public int EconomicOrderQuantity =>
            AnnualDemand.HasValue && AnnualDemand > 0 && OrderingCost.HasValue && OrderingCost > 0 && AnnualHoldingCost > 0
                ? (int)Math.Sqrt((2 * AnnualDemand.Value * (double)OrderingCost.Value) / (double)AnnualHoldingCost)
                : 0;

        [NotMapped]
        public decimal PeriodicOrderQuantity => EconomicOrderQuantity > 0 && AnnualDemand.HasValue
            ? EconomicOrderQuantity / (AnnualDemand.Value / 12m)
            : 0;
    }
}
