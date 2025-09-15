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

    public async Task<BoardUser?> GetBoardOwner(Guid boardId)
    {
        return await _context.BoardUsers
            .Where(u => u.BoardId == boardId && u.Role == UserAccessEnum.Owner)
            .FirstOrDefaultAsync();
    }

}
