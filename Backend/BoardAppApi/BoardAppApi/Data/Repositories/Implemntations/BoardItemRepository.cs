using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BoardAppApi.Data.Repositories.Implemntations
{
    public class BoardItemRepository : BaseRepository<BoardItem>
    {
        public BoardItemRepository(BoardAppDbContext context) : base(context)
        {
        }

        public async Task<List<BoardItem>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Set<BoardItem>().ToListAsync(cancellationToken);
        }
    }
}
