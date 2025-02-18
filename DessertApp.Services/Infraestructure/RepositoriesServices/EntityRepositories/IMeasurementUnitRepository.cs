using DessertApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories
{
    public interface IMeasurementUnitRepository
    {
        Task<IEnumerable<MeasurementUnit>> GetMeasurementUnitsAsync(CancellationToken cancellationToken);
    }
}
