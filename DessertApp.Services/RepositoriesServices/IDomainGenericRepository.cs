using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Services.RepositoriesServices
{
    public interface IDomainGenericRepository<T, TKey> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<T> GetByFieldAsync(string fieldName, string value, CancellationToken cancellationToken);
        Task<T> DeleteAsync(T entity, CancellationToken cancellationToken, params object[] releatedEntities);
        Task<T> GetByIdWithDetailsAsync(TKey id, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes);
    }
}
