using System.Linq.Expressions;

namespace DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories
{
    public interface IDomainGenericRepository<T, TKey> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        void Attach(T entity, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<T?> GetByFieldAsync(string fieldName, string value, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken, params object[] releatedEntities);
        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllWithDetailsAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T?> GetByIdWithDetailsAsync(TKey id, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
