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
        public int Stock {  get; set; }

        public bool IsAvailable { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal CostPerUnit { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal OrderingCost {  get; set; }

        [Range(0.0, 1.0)]
        public decimal MonthlyHoldingCostRate {  get; set; }

        [NotMapped]
        //Calculated property to ensure best analysis
        public decimal AnnualHoldingCost => CostPerUnit * MonthlyHoldingCostRate * 12;

        public int AnnualDemand {  get; set; }

        [NotMapped]
        //Calculated property for EOQ
        public int EconomicOrderQuantity =>
            AnnualDemand > 0 && OrderingCost > 0 && AnnualHoldingCost > 0
                ? (int)Math.Sqrt((2 * AnnualDemand * (double)OrderingCost) / (double)AnnualHoldingCost)
                : 0;

        [NotMapped]
        public decimal PeriodicOrderQuantity => EconomicOrderQuantity / (AnnualDemand / 12m);
    }
}
