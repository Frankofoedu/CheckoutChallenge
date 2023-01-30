using Microsoft.EntityFrameworkCore;
using PaymentGateway.Core.Data;
using System.Linq.Expressions;

namespace PaymentGateway.Core.DataAccess
{
    public class EfRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly ApplicationDbContext DbContext;
        public readonly DbSet<T> Table;

        public EfRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;

            Table = DbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return GetAllIncluding();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (propertySelectors == null || propertySelectors.Length < 1) return query;

            return propertySelectors.Aggregate(query, (current, propertySelector) => current.Include(propertySelector));
        }

        public async Task<T> GetByIdAsync<TId>(TId id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _ = entities.Select(x => { DbContext.Entry(x).State = EntityState.Modified; return x; });
            await DbContext.SaveChangesAsync();
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            await DbContext.Set<T>().AddRangeAsync(entities);
            await DbContext.SaveChangesAsync();

            return entities;
        }

        public async Task DeleteAsync(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);

            await DbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}