using System.Linq.Expressions;
using Board.Application.Abstractions.Repositories;
using Board.Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Board.Infrastructure.Data.Repositories;

public abstract class Repository<T> : IRepository<T>
        where T : class
{
    protected BoardDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(BoardDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {

        EntityEntry<T> result = await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        EntityEntry<T> result = _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task UpdateRangeAsync(T[] entities, CancellationToken cancellationToken)
    {
        _dbSet.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<T> DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        EntityEntry<T> result = _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<List<T>> FindAsync(
        string searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken,
        params Expression<Func<T, string>>[] properties)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return [];
        }

        Expression<Func<T, bool>> predicate = x => false;

        foreach (Expression<Func<T, string>> property in properties)
        {
            ParameterExpression param = property.Parameters[0];

            BinaryExpression notNull = Expression.NotEqual(property.Body, Expression.Constant(null, typeof(string)));
            MethodCallExpression contains = Expression.Call(
                property.Body,
                nameof(string.Contains),
                Type.EmptyTypes,
                Expression.Constant(searchTerm, typeof(string))
            );
            BinaryExpression andExpr = Expression.AndAlso(notNull, contains);

            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(andExpr, param);

            predicate = OrElse(predicate, lambda);
        }

        return await _dbSet
            .AsNoTracking()
            .Where(predicate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }


    public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return _dbSet.AsNoTracking()
                     .CountAsync(predicate, cancellationToken);
    }

    public Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
    {
        return _dbSet.AsNoTracking(asNoTracking)
                     .Includes(includes)
                     .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
    {
        return await _dbSet.AsNoTracking(true)
                           .WhereIf(predicate)
                           .Includes(includes)
                           .Select(selector)
                           .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
    {
        return await _dbSet.AsNoTracking(asNoTracking)
                           .Includes(includes)
                           .ToListAsync(cancellationToken);
    }

    public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
    {
        return await _dbSet.AsNoTracking(asNoTracking)
                           .WhereIf(predicate)
                           .Includes(includes)
                           .ToListAsync(cancellationToken);
    }

    public async Task<IList<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
    {
        return await _dbSet.AsNoTracking(true)
                           .WhereIf(predicate)
                           .Includes(includes)
                           .Select(selector)
                           .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static Expression<Func<T, bool>> OrElse(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T));

        BinaryExpression body = Expression.OrElse(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

}
