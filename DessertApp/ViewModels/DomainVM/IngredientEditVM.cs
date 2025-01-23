using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DessertApp.ViewModels.DomainVM
{
    public class IngredientEditVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The value must be a positive number.")]
        public int? Stock {  get; set; }

        public IngredientUnitVM IngredientUnitVM { get; set; }

        public SelectList? AvailableUnits {  get; set; } 
    }
}
