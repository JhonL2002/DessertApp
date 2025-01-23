using System.ComponentModel.DataAnnotations;

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

        public IngredientUnit? IngredientUnit { get; set; }
    }
}