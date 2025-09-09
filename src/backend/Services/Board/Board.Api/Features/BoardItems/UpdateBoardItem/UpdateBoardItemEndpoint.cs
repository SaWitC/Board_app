using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemEndpoint : Endpoint<UpdateBoardItemRequest>
{
    private readonly IRepository<BoardItem> _repository;

    public UpdateBoardItemEndpoint(IRepository<BoardItem> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Put("/api/boarditems/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBoardItemRequest request, CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        BoardItem entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        if (entity == null)
        {
            await Send.OkAsync(null, cancellationToken);
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.BoardColumnId = request.BoardColumnId;
        entity.Priority = request.Priority;
        entity.AssigneeId = request.AssigneeId;
        entity.DueDate = request.DueDate;
        entity.ModificationDate = DateTimeOffset.UtcNow;

        BoardItem updated = await _repository.UpdateAsync(entity, cancellationToken);
        BoardItemDto response = new BoardItemDto
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

        await Send.OkAsync(response, cancellationToken);
    }
}


