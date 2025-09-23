using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs.BoardItems;
using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Data.Repositories;
public class BoardItemRepository : Repository<BoardItem>, IBoardItemRepository
{
    public BoardItemRepository(BoardDbContext context) : base(context)
    {
    }

    public async Task<ICollection<BoardItemLokupDto>> GetAllBoardItemsLookup(Guid boardId, CancellationToken cancellationToken)
    {
        return await _context.Boards
        .Where(x => x.Id == boardId)
        .SelectMany(x => x.BoardColumns)
        .SelectMany(x => x.Elements)
        .Select(x => new BoardItemLokupDto()
        {
            Id = x.Id,
            Title = x.Title,
            BoardColumnId = x.BoardColumnId,
            Priority = x.Priority,
            AssigneeId = x.AssigneeId,
            DueDate = x.DueDate,
            ModificationDate = x.ModificationDate,
            CreatedTime = x.CreatedTime,
            TaskType = x.TaskType,
        }).ToListAsync(cancellationToken);

    }
}
