using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;
using MediatR;

namespace BoardAppApi.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemHandler(IRepository<BoardItem> _rep) : IRequestHandler<UpdateBoardItemCommand, BoardItem>
{
    public async Task<BoardItem> Handle(UpdateBoardItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _rep.GetAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.BoardColumnId = request.BoardColumnId;
        entity.Priority = request.Priority;
        entity.AssigneeId = request.AssigneeId;
        entity.DueDate = request.DueDate;
        entity.ModificationDate = DateTimeOffset.UtcNow;

        return await _rep.UpdateAsync(entity, cancellationToken);
    }
}


