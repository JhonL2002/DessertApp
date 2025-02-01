using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DessertApp.Models.Entities
{
    public class Dessert
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock {  get; set; }

        public bool IsAvailable { get; set; }

        public int? AnnualDemand { get; set; }

        public int DessertCategoryId { get; set; }

        //One dessert belongs to one category
        public DessertCategory DessertCategory { get; set; }

        [NotMapped]
        public bool IsLowDemand => AnnualDemand.HasValue && AnnualDemand < 100;
    }
}
