using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.Repositories
{
    public interface IGenericRepository<T,TResult,TKey>
        where T : class
        where TResult : class
        where TKey : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken);
        Task<TResult> CreateAsync(T entity, CancellationToken cancellationToken);
        Task<TResult> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<TResult> DeleteAsync (T entity, CancellationToken cancellationToken);
        Task<T> GetDetailsAsync(TKey id, CancellationToken cancellationToken);
    }
}
