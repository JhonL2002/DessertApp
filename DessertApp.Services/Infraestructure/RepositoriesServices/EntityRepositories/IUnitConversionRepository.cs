using DessertApp.Models.Entities;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories
{
    public interface IUnitConversionRepository
    {
        Task<IEnumerable<UnitConversion>> GetAllUnitConversion(CancellationToken cancellationToken);
    }
}
