using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Entities;
using Board.Domain.Security;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardItems.CreateBoardItem;

public class CreateBoardItemEndpoint : Endpoint<CreateBoardItemRequest>
{
    private readonly IBoardItemRepository _repository;
    private readonly IMapper _mapper;

    public CreateBoardItemEndpoint(IBoardItemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Post("/api/boards/{boardId}/columns/{boardColumnId}/items");
        Policies(Auth.BuildPermissionPolicy(Permission.ManageItems, Context.BoardItem, "boardId"));
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
            ModificationDate = DateTimeOffset.UtcNow,
            TaskType = request.TaskType
        };

        BoardItem created = await _repository.AddAsync(entity, cancellationToken);
        BoardItemDto response = _mapper.Map<BoardItemDto>(created);

        await Send.OkAsync(response, cancellationToken);
    }
}


