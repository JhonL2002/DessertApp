using DessertApp.Models.Entities;
using DessertApp.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.Application.DessertServices
{
    public interface IDessertDemandService
    {
        Task<List<DessertDemandsImportlDTO>> GetDessertDemands(List<DessertAnalysis> importedDessertAnalysis, CancellationToken cancellationToken);
    }
}
