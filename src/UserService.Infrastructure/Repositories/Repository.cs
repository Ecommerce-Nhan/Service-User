using Microsoft.EntityFrameworkCore;
using SharedLibrary.Repositories.Abtractions;

namespace UserService.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly UserDbContext Context;
    protected readonly DbSet<T> DbSet;
    public Repository(UserDbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }
    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }
    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
        await Context.SaveChangesAsync();
    }
    public IQueryable<T> GetQueryable()
    {
        return DbSet.AsQueryable();
    }
    public async Task Remove(T entity)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
    public async Task RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
        await Context.SaveChangesAsync();
    }
    public async Task Update(T entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }
    public async Task UpdateRange(IEnumerable<T> entities)
    {
        DbSet.UpdateRange(entities);
        await Context.SaveChangesAsync();
    }
}
