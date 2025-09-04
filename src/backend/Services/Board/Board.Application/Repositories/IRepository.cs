using Board.Domain.Entities.Abstractions;

namespace Board.Application.Repositories;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    public Task<TEntity> GetAsync(Guid Id, CancellationToken cancellationToken);
    public Task<TEntity> InsertAsync(TEntity Entity, CancellationToken cancellationToken);
    public Task<TEntity> UpdateAsync(TEntity Entity, CancellationToken cancellationToken);
    public Task<TEntity> DeleteAsync(TEntity Entity, CancellationToken cancellationToken);
}
