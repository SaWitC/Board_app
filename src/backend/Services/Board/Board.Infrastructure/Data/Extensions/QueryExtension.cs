using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Data.Extensions;

internal static class QueryExtension
{
    public static IQueryable<T> Includes<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes)
        where T : class
    {
        if (includes != null && includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query;
    }

    public static IQueryable<T> AsNoTracking<T>(this IQueryable<T> query, bool asNoTracking)
        where T : class
    {
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        where T : class
    {
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return query;
    }

}
