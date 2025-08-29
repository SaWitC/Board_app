using BoardAppApi.Data.Entities.Abstractions;

namespace BoardAppApi.Data.Repositories.Abstractions
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly BoardAppDbContext context;
        public BaseRepository(BoardAppDbContext context)
        {
            this.context = context;
        }

        public async Task<TEntity> DeleteAsync(TEntity Entity, CancellationToken cancellationToken)
        {
            var entity = context.Set<TEntity>().Remove(Entity);
            await context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<TEntity> GetAsync(Guid Id,CancellationToken cancellationToken)
        {
            return await context.Set<TEntity>().FindAsync(Id,cancellationToken);
        }

        public async Task<TEntity> InsertAsync(TEntity Entity, CancellationToken cancellationToken)
        {
            var entity = await context.Set<TEntity>().AddAsync(Entity, cancellationToken);
            await context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity Entity, CancellationToken cancellationToken)
        {
            var entity = context.Set<TEntity>().Update(Entity);
            await context.SaveChangesAsync();
            return entity.Entity;
        }
    }
}
