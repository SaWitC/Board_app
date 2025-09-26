using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs.BoardItems;
using Board.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Data.Repositories;
public class BoardItemRepository : Repository<BoardItem>, IBoardItemRepository
{
    public BoardItemRepository(BoardDbContext context) : base(context)
    {
    }

    public async Task<ICollection<BoardItemLookupDto>> GetAllBoardItemsLookup(Guid boardId, CancellationToken cancellationToken)
    {
        return await _context.Boards
        .Where(x => x.Id == boardId)
        .SelectMany(x => x.BoardColumns)
        .SelectMany(x => x.Items)
        .Select(x => new BoardItemLookupDto()
        {
            Id = x.Id,
            Title = x.Title,
            BoardColumnId = x.BoardColumnId,
            BoardId = x.BoardId,
            Priority = x.Priority,
            AssigneeEmail = x.AssigneeEmail,
            DueDate = x.DueDate,
            ModificationDate = x.ModificationDate,
            CreatedTime = x.CreatedTime,
            TaskType = x.TaskType,
        }).ToListAsync(cancellationToken);

    }
}
