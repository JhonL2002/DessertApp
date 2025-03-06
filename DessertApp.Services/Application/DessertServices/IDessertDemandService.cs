using DessertApp.Models.Entities;
using DessertApp.Services.DTOs;

namespace DessertApp.Services.Application.DessertServices
{
    public interface IDessertDemandService
    {
        Task<List<DessertDemandsImportlDTO>> GetDessertDemands(List<DessertAnalysis> importedDessertAnalysis, CancellationToken cancellationToken);
    }
}
