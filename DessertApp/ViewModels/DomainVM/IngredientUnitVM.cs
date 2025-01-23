using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DessertApp.ViewModels.DomainVM
{
    public class IngredientUnitVM
    {
        public int Id { get; set; }

        public int IngredientId { get; set; }

        public int SelectedUnitId { get; set; }

        [RegularExpression(@"^\d{1,18}(\.\d{1,2})?$", ErrorMessage = "The value must be a positive number.")]
        public int? ItemsPerUnit { get; set; }

        [RegularExpression(@"^\d{1,18}(\.\d{1,2})?$", ErrorMessage = "The value must be a positive number.")]
        public decimal? CostPerUnit { get; set; }

        [RegularExpression(@"^\d{1,18}(\.\d{1,2})?$", ErrorMessage = "The value must be a positive number.")]
        public decimal? OrderingCost { get; set; }

        [RegularExpression(@"^\d{1,18}(\.\d{1,2})?$", ErrorMessage = "The value must be a positive number.")]
        public decimal? MonthlyHoldingCostRate { get; set; }

        [RegularExpression(@"^\d{1,18}(\.\d{1,2})?$", ErrorMessage = "The value must be a positive number.")]
        public int? AnnualDemand { get; set; }

        public IEnumerable<SelectListItem>? MeasurementsUnits { get; set; }
    }
}
