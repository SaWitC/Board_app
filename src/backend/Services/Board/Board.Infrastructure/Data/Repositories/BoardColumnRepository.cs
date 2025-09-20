using Board.Application.Abstractions.Repositories;
using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Data.Repositories;
public class BoardColumnRepository : Repository<BoardColumn>, IBoardColumnRepository
{
    public BoardColumnRepository(BoardDbContext context) : base(context)
    {
    }

    public async Task<IList<BoardColumn>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<BoardColumn>()
            .Where(x => x.BoardId == boardId)
            .ToListAsync(cancellationToken);
    }
}
