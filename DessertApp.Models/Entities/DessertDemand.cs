
using System.ComponentModel.DataAnnotations;

namespace DessertApp.Models.Entities
{
    public class DessertDemand
    {
        public int Id { get; set; }

        public int DessertId { get; set; }
        public Dessert Dessert { get; set; }

        public string Month { get; set; }

        [Range(0, int.MaxValue)]
        public int Demand { get; set; }
    }
}
