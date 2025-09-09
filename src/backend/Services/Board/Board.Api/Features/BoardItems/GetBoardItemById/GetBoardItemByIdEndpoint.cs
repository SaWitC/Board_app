using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardItems.GetBoardItemById;

public class GetBoardItemByIdEndpoint : Endpoint<GetBoardItemByIdRequest>
{
    private readonly IRepository<BoardItem> _repository;

    public GetBoardItemByIdEndpoint(IRepository<BoardItem> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Get("/api/boarditems/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetBoardItemByIdRequest request, CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        BoardItem entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        if (entity == null)
        {
            await Send.OkAsync(null, cancellationToken);
        }

        BoardItemDto response = new BoardItemDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            BoardColumnId = entity.BoardColumnId,
            Priority = entity.Priority,
            AssigneeId = entity.AssigneeId,
            DueDate = entity.DueDate,
            ModificationDate = entity.ModificationDate,
            CreatedTime = entity.CreatedTime
        };
        await Send.OkAsync(response, cancellationToken);
    }
}
