namespace PaymentGateway.Core.DataAccess
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task UpdateRangeAsync(IEnumerable<T> entities);

        Task DeleteAsync(T entity);

        Task<List<T>> AddRangeAsync(List<T> entities);

        Task DeleteRangeAsync(IEnumerable<T> entities);

        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdAsync<TId>(TId id);

        IQueryable<T> GetAll();

        Task<IReadOnlyList<T>> ListAllAsync();

        Task SaveChangesAsync();
    }
}