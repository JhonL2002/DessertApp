using DessertApp.Services.RepositoriesServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            //First, add main entity
            await _dbSet.AddAsync(entity, cancellationToken);

            //Commit changes
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<T> DeleteAsync(T entity, CancellationToken cancellationToken, params object[] releatedEntities)
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
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken)
        {
            return (await _dbSet.FindAsync([id], cancellationToken))!;
        }

        public async Task<T> GetByIdWithDetailsAsync(TKey id, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(entity => EF.Property<TKey>(entity, "Id")!.Equals(id), cancellationToken)
                ?? throw new KeyNotFoundException($"Entity with Id {id} not found.");
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            //Update main entity
            _dbSet.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}
