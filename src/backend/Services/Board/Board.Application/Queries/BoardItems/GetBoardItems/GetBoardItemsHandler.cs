using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using MediatR;

namespace Board.Application.Queries.BoardItems.GetBoardItems;

public class GetBoardItemsHandler : IRequestHandler<GetBoardItemsQuery, List<BoardItemDto>>
{
    private readonly IRepository<BoardItem> _repository;

    public GetBoardItemsHandler(IRepository<BoardItem> repository)
    {
        _repository = repository;
    }

    public async Task<List<BoardItemDto>> Handle(GetBoardItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(null, e => new BoardItemDto
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
        return [.. items];
    }
}
