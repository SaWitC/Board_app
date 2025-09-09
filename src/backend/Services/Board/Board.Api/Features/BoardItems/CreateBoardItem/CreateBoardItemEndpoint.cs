using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardItems.CreateBoardItem;

public class CreateBoardItemEndpoint : Endpoint<CreateBoardItemRequest>
{
    private readonly IRepository<BoardItem> _repository;

    public CreateBoardItemEndpoint(IRepository<BoardItem> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Post("/api/boarditems");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBoardItemRequest request, CancellationToken cancellationToken)
    {
        BoardItem entity = new BoardItem
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

        BoardItem created = await _repository.AddAsync(entity, cancellationToken);
        BoardItemDto response = new BoardItemDto
        {
            Id = created.Id,
            Title = created.Title,
            Description = created.Description,
            BoardColumnId = created.BoardColumnId,
            Priority = created.Priority,
            AssigneeId = created.AssigneeId,
            DueDate = created.DueDate,
            CreatedTime = created.CreatedTime,
            ModificationDate = created.ModificationDate
        };

        await Send.OkAsync(response, cancellationToken);
    }
}


