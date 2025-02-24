using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;

namespace DessertApp.Infraestructure.EntityRepositories
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
