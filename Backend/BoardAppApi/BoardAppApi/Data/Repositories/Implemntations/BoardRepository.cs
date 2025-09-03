using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BoardAppApi.Data.Repositories.Implemntations
{
    public class BoardRepository : BaseRepository<Board>
    {
        public BoardRepository(BoardAppDbContext context) : base(context)
        {
        }

        public async Task<List<Board>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Set<Board>().ToListAsync(cancellationToken);
        }
    }
}
