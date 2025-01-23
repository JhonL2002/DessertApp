using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using DessertApp.Services.UnitOfWorkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Application.MeasurementUnitServices
{
    public class MeasurementUnitService : IMeasurementUnitService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MeasurementUnitService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MeasurementUnit>> GetMeasurementUnitsAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.MeasurementUnits.GetAllAsync(cancellationToken);
        }
    }
}
