using Board.Domain.Entities.Abstractions;
using Board.Application.Repositories;

namespace Board.Infrastructure.Data.Repositories.Abstractions;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly BoardDbContext context;
    public BaseRepository(BoardDbContext context)
    {
        this.context = context;
    }

    public async Task<TEntity> DeleteAsync(TEntity Entity, CancellationToken cancellationToken)
    {
        var entity = context.Set<TEntity>().Remove(Entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Entity;
    }

    public async Task<TEntity> GetAsync(Guid Id,CancellationToken cancellationToken)
    {
        return await context.Set<TEntity>().FindAsync(Id,cancellationToken);
    }

    public async Task<TEntity> InsertAsync(TEntity Entity, CancellationToken cancellationToken)
    {
        var entity = await context.Set<TEntity>().AddAsync(Entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity Entity, CancellationToken cancellationToken)
    {
        var entity = context.Set<TEntity>().Update(Entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Entity;
    }
}
