using System.ComponentModel.DataAnnotations;

namespace DessertApp.Models.Entities
{
    public class DessertCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        //Lot of desserts belong to one DessertCategory
        public ICollection<Dessert> Desserts { get; set; } = new List<Dessert>();
    }
}
