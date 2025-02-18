using DessertApp.Infraestructure.Data;
using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Infraestructure.DomainRepositories
{
    public class UnitConversionRepository : IUnitConversionRepository
    {
        private readonly DomainPersistentRepository<UnitConversion, int> _unitConversionRepository;

        public UnitConversionRepository(DomainPersistentRepository<UnitConversion, int> unitConversionRepository)
        {
            _unitConversionRepository = unitConversionRepository;
        }

        public async Task<IEnumerable<UnitConversion>> GetAllUnitConversion(CancellationToken cancellationToken)
        {
            return await _unitConversionRepository.GetAllWithDetailsAsync(
                filter: null,
                cancellationToken: cancellationToken,
                include: query => query.Include(uc => uc.FromUnit).Include(uc => uc.ToUnit)
            );
                
        }
    }
}
