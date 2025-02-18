using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.DTOs
{
    public class IngredientCalculationDTO
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int EOQ { get; set; }
        public decimal OrderFrequency { get; set; }
        public decimal OptimalOrderingPeriod { get; set; }
        public decimal AnnualDemand { get; set; }
        public decimal OrderingCost { get; set; }
        public decimal CostOfMaintainingUnitsInInventory { get; set; }
    }
}
