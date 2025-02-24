using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Infraestructure.EntityRepositories
{
    public class UnitConversionRepository : IUnitConversionRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitConversionRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UnitConversion>> GetAllUnitConversion(CancellationToken cancellationToken)
        {
            return await _unitOfWork.UnitConversions.GetAllWithDetailsAsync(
                filter: null,
                cancellationToken: cancellationToken,
                include: query => query.Include(uc => uc.FromUnit).Include(uc => uc.ToUnit)
            );
        }
    }
}
