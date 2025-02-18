using DessertApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.Infraestructure.CacheServices
{
    public interface IDessertCategoryCacheService
    {
        Task<IEnumerable<DessertCategory>> GetCategoriesWithDessertsAsync(CancellationToken cancellationToken);
        void RemoveCategoriesCache();
    }
}
