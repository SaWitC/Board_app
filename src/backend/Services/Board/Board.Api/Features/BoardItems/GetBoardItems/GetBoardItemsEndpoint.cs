using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardItems.GetBoardItems;

public class GetBoardItemsEndpoint : Endpoint<GetBoardItemsRequest>
{
    private readonly IRepository<BoardItem> _repository;

    public GetBoardItemsEndpoint(IRepository<BoardItem> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/api/boarditems");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetBoardItemsRequest req, CancellationToken cancellationToken)
    {
        IList<BoardItemDto> items = await _repository.GetAllAsync(null, e => new BoardItemDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            BoardColumnId = e.BoardColumnId,
            Priority = e.Priority,
            AssigneeId = e.AssigneeId,
            DueDate = e.DueDate,
            ModificationDate = e.ModificationDate,
            CreatedTime = e.CreatedTime
        }, cancellationToken);

        await Send.OkAsync(items, cancellationToken);
    }
}


