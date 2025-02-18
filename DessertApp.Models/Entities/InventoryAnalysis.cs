using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Models.Entities
{
    public class InventoryAnalysis
    {
        public int Id { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } //Name for auditory purposes
        public int EOQ { get; set; }
        public decimal OrderFrequency { get; set; }
        public decimal OptimalOrderingPeriod { get; set; }
        public decimal AnnualDemand { get; set; }
        public decimal OrderingCost { get; set; }
        public decimal CostOfMaintainingUnitsInInventory { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}
