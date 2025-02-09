using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using DessertApp.Services.UnitOfWorkServices;

namespace DessertApp.Application.MeasurementUnitServices
{
    public class MeasurementUnitRepository : IMeasurementUnitRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public MeasurementUnitRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MeasurementUnit>> GetMeasurementUnitsAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.MeasurementUnits.GetAllAsync(cancellationToken);
        }
    }
}
