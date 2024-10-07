using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MyTrades.WebApi;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public DbSet<T> Entity => _dbSet;

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if(entity is null)
            throw new EntityNotFoundException(id, $"Entity not found with the id {id}");
        
        return entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(Guid id, T entity)
    {
        var foundEntity = await GetByIdAsync(id);
        _context.Entry(foundEntity).CurrentValues.SetValues(entity);
    }

    public void Delete(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        Delete(entity);        
    }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<TResult>> JoinAsync<TJoin, TKey, TResult>(
        Expression<Func<T, TKey>> outerKeySelector,
        Expression<Func<TJoin, TKey>> innerKeySelector,
        Expression<Func<T, TJoin, TResult>> resultSelector)
        where TJoin : class
    {
        return await _dbSet.Join(
            _context.Set<TJoin>(),
            outerKeySelector,
            innerKeySelector,
            resultSelector
        ).ToListAsync();
    }
}
