using System.Linq.Expressions;
using Board.Application.Interfaces;
using Board.Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Data.Repositories;

public class Repository<T> : IRepository<T>
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
         
        var result = await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var result = _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<T> DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        var result = _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
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
}
