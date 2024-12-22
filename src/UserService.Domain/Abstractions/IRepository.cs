namespace UserService.Domain.Abstractions;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetQueryable();
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task Remove(T entity);
    Task RemoveRange(IEnumerable<T> entities);
    Task Update(T entity);
    Task UpdateRange(IEnumerable<T> entities);
}