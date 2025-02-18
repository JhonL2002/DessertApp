using DessertApp.Models.Entities;
using DessertApp.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.Application.UnitConversionServices
{
    public interface IUnitConversionService
    {
        List<DessertDemandsImportlDTO> ConvertUnits(List<DessertDemandsImportlDTO> orderDetailsDTO, List<UnitConversion> conversions);
    }
}
