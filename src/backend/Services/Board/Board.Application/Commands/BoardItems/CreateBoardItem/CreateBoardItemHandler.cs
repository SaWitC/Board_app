using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using MediatR;

namespace Board.Application.Commands.BoardItems.CreateBoardItem;

public class CreateBoardItemHandler : IRequestHandler<CreateBoardItemCommand, BoardItemDto>
{
    private readonly IRepository<BoardItem> _repository;

    public CreateBoardItemHandler(IRepository<BoardItem> repository)
    {
        _repository = repository;
    }

    public async Task<BoardItemDto> Handle(CreateBoardItemCommand request, CancellationToken cancellationToken)
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

        var created = await _repository.AddAsync(entity, cancellationToken);
        return new BoardItemDto
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
    }
}
