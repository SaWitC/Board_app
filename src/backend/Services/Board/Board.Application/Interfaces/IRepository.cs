using System.Linq.Expressions;

namespace Board.Application.Interfaces;

public interface IRepository<T>
        where T : class
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);

    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);

    Task<T> DeleteAsync(T entity, CancellationToken cancellationToken);    

    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

    Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, bool asNoTracking = true, params Expression<Func<T, object>>[] includes);

    Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes);

    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = true, params Expression<Func<T, object>>[] includes);

    Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, bool asNoTracking = true, params Expression<Func<T, object>>[] includes);

    Task<IList<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes);
}
