using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Implemntations;
using MediatR;

namespace BoardAppApi.Features.BoardItems.CreateBoard;

public class CreateBoardItemHandler(BoardItemRepository _rep) : IRequestHandler<CreateBoardItemCommand, BoardItem>
{
    public async Task<BoardItem> Handle(CreateBoardItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new BoardItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            BoardColumnId = request.BoardColumnId,
            Priority = request.Priority,
            AssigneeId = request.AssigneeId,
            DueDate = request.DueDate,
            CreatedTime = DateTime.UtcNow,
            ModificationDate = DateTimeOffset.UtcNow
        };

        return await _rep.InsertAsync(entity, cancellationToken);
    }
}


