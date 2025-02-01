using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DessertApp.Models.Entities
{
    public class DessertIngredient
    {
        public int Id { get; set; }

        public int DessertId { get; set; }
        public Dessert Dessert { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        //Minimum quantity required
        [Range(0.01, double.MaxValue)]
        public decimal QuantityRequired { get; set; }

        //Foreign key to MeasurementUnit
        public int UnitId { get; set; }
        public MeasurementUnit Unit { get; set; }
    }
}
