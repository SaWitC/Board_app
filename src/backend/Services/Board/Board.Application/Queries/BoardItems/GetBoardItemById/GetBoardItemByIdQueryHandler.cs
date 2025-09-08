using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using MediatR;

namespace Board.Application.Queries.BoardItems.GetBoardItemById;

public class GetBoardItemByIdQueryHandler : IRequestHandler<GetBoardItemByIdQuery, BoardItemDto>
{
    private readonly IRepository<BoardItem> _repository;

    public GetBoardItemByIdQueryHandler(IRepository<BoardItem> repository)
    {
        _repository = repository;
    }

    public async Task<BoardItemDto> Handle(GetBoardItemByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        return new BoardItemDto
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
    }
}
