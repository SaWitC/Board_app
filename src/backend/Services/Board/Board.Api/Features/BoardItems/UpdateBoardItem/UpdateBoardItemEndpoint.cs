using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs.BoardItems;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Entities;
using Board.Domain.Security;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemEndpoint : Endpoint<UpdateBoardItemRequest>
{
    private readonly IBoardItemRepository _repository;
    private readonly IMapper _mapper;

    public UpdateBoardItemEndpoint(IBoardItemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Put("/api/boards/{boardId}/columns/{boardColumnId}/items/{id}");
        Policies(Auth.BuildPermissionPolicy(Permission.ManageItems, Context.BoardItem, "boardId"));
    }

    public override async Task HandleAsync(UpdateBoardItemRequest request, CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        BoardItem entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        if (entity == null)
        {
            			await Send.NotFoundAsync(cancellationToken);
			return;
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.BoardColumnId = request.BoardColumnId;
        entity.Priority = request.Priority;
        entity.AssigneeId = request.AssigneeId;
        entity.DueDate = request.DueDate;
        entity.TaskType = request.TaskType;
        entity.ModificationDate = DateTimeOffset.UtcNow;

        BoardItem updated = await _repository.UpdateAsync(entity, cancellationToken);
        BoardItemDto response = _mapper.Map<BoardItemDto>(updated);

        await Send.OkAsync(response, cancellationToken);
    }
}
