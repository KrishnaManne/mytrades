using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MyTrades.WebApi;

public interface IGenericRepository<T> where T : class
{
    DbSet<T> Entity {get;}
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<TResult>> JoinAsync<TJoin, TKey, TResult>(
        Expression<Func<T, TKey>> outerKeySelector,
        Expression<Func<TJoin, TKey>> innerKeySelector,
        Expression<Func<T, TJoin, TResult>> resultSelector)
        where TJoin : class;
}
