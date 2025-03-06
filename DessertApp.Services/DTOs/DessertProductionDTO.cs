using DessertApp.Models.Entities;

namespace DessertApp.Services.DTOs
{
    public class DessertProductionDTO
    {
        public int IngredientId { get; set; }
        public MeasurementUnit Unit { get; set; }
        public decimal Quantity { get; set; }
    }
}
