using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DessertApp.ViewModels.DomainVM
{
    public class DessertEditVM
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "The price must be a positive number.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number.")]
        public int Stock { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Annual demand must be a non-negative number.")]
        public int AnnualDemand { get; set; } = 0;

        [Required(ErrorMessage = "You must select a category.")]
        public int SelectedCategory { get; set; }

        public SelectList? DessertCategories { get; set; }
    }
}
