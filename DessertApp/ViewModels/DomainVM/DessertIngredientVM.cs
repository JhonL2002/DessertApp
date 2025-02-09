using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DessertApp.ViewModels.DomainVM
{
    public class DessertIngredientVM
    {
        [Range(0, double.MaxValue, ErrorMessage = "The quantity required must be a positive number.")]
        public decimal QuantityRequired { get; set; }

        [Required(ErrorMessage = "You must select an ingredient")]
        public int IngredientId { get; set; }
        public SelectList? Ingredients { get; set; }

        [Required(ErrorMessage = "You must select a dessert")]
        public int DessertId { get; set; }
        public SelectList? Desserts { get; set; }

        [Required(ErrorMessage = "You must select a measurement unit")]
        public int UnitId { get; set; }
        public SelectList? Units { get; set; }
    }
}
