using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Models.Entities
{
    public class UnitConversion
    {
        public int Id { get; set; }
        public int FromUnitId { get; set; }
        public int ToUnitId { get; set; }
        public decimal ConversionFactor { get; set; }
        public bool IsReversible { get; set; }

        public MeasurementUnit FromUnit { get; set; }
        public MeasurementUnit ToUnit { get; set; }
    }
}
