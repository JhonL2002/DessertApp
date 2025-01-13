using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Models.Entities
{
    public class DessertIngredient
    {
        public int Id { get; set; }
        public int DessertId { get; set; }
        public int IngredientId { get; set; }

        //Minimum quantity required
        [Range(1, int.MaxValue)]
        public int QuantityRequired { get; set; }

        //Foreign key to MeasurementUnit
        public int UnitId { get; set; }
        public MeasurementUnit Unit { get; set; }
    }
}
