using Board.Application.Abstractions.Repositories;
using Board.Domain.Contracts.Enums;
using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Data.Repositories;
public class BoardRepository : Repository<Domain.Entities.Board>, IBoardRepository
{
    public BoardRepository(BoardDbContext context) : base(context)
    {
    }

    public async Task<BoardUser> GetBoardOwnerAsync(Guid boardId, CancellationToken cancellationToken)
    {
        return await _context.Boards
            .Where(b => b.Id == boardId)
            .SelectMany(b => b.BoardUsers)
            .FirstOrDefaultAsync(u => u.Role == UserAccessEnum.Owner, cancellationToken: cancellationToken);
    }
}
