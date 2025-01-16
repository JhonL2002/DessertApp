using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Models.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int? Stock {  get; set; }

        public bool IsAvailable { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal? CostPerUnit { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal? OrderingCost {  get; set; }

        [Range(0.0, 1.0)]
        public decimal? MonthlyHoldingCostRate {  get; set; }

        [NotMapped]
        //Calculated property to ensure best analysis
        public decimal AnnualHoldingCost => CostPerUnit.HasValue && MonthlyHoldingCostRate.HasValue
            ? CostPerUnit.Value * MonthlyHoldingCostRate.Value * 12
            : 0;

        public int? AnnualDemand {  get; set; }

        [NotMapped]
        //Calculated property for EOQ
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
