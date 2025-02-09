using DessertApp.Services.RepositoriesServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DessertApp.Infraestructure.Data
{
    public class DomainPersistentRepository<T, TKey> : IDomainGenericRepository<T, TKey> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public DomainPersistentRepository(
            AppDbContext context
            )
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            //First, add main entity
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Attach(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Attach(entity);
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken, params object[] releatedEntities)
        {
            //Delete releated entities
            foreach (var releatedEntity in releatedEntities)
            {
                if (releatedEntity != null)
                {
                    _context.Entry(releatedEntity).State = EntityState.Deleted;
                }
            }
            //Then, delete main entity
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentException("The entities collection cannot be null or be empty");
            }

            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllWithDetailsAsync(
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);

            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByFieldAsync(string fieldName, string value, CancellationToken cancellationToken)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, fieldName);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);
            var predicate = Expression.Lambda<Func<T,bool>>(equality, parameter);

            return await _dbSet
                .Where(predicate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public async Task<T?> GetByIdWithDetailsAsync(TKey id, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(entity => EF.Property<TKey>(entity, "Id")!.Equals(id), cancellationToken)
                ?? throw new KeyNotFoundException($"Entity with Id {id} not found.");
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            //Update main entity
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
    }
}
