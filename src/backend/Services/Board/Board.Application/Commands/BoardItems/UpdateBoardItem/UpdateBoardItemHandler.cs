using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using MediatR;

namespace Board.Application.Commands.BoardItems.UpdateBoardItem;

public class UpdateBoardItemHandler : IRequestHandler<UpdateBoardItemCommand, BoardItemDto>
{
    private readonly IRepository<BoardItem> _repository;

    public UpdateBoardItemHandler(IRepository<BoardItem> repository)
    {
        _repository = repository;
    }

    public async Task<BoardItemDto> Handle(UpdateBoardItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken);
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

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return new BoardItemDto
        {
            Id = updated.Id,
            Title = updated.Title,
            Description = updated.Description,
            BoardColumnId = updated.BoardColumnId,
            Priority = updated.Priority,
            AssigneeId = updated.AssigneeId,
            DueDate = updated.DueDate,
            ModificationDate = updated.ModificationDate,
            CreatedTime = updated.CreatedTime
        };
    }
}
