using Board.Application.DTOs.BoardItems;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Entities;
using Board.Domain.Security;
using Board.Infrastructure.Data.UoW;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemEndpoint : Endpoint<UpdateBoardItemRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBoardItemEndpoint(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
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
        Guid boardId = Route<Guid>("boardId");

        BoardItem entity = await _unitOfWork.BoardItems.GetAsync(x => x.Id == id, cancellationToken, false, x => x.Tags);
        if (entity == null)
        {
            await Send.NotFoundAsync(cancellationToken);
            return;
        }
        var tagsForEntity = await _unitOfWork.Tags.GetOrCreateTagsAsync(request.Tags, cancellationToken);

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.BoardColumnId = request.BoardColumnId;
        entity.BoardId = boardId;
        entity.Priority = request.Priority;
        entity.AssigneeEmail = request.AssigneeEmail;
        entity.DueDate = request.DueDate;
        entity.TaskType = request.TaskType;
        entity.ModificationDate = DateTimeOffset.UtcNow;
        entity.Tags = tagsForEntity;

        var item = await _unitOfWork.BoardItems.UpdateAsync(entity, cancellationToken);

        var response = _mapper.Map<BoardItemDto>(item);
        await Send.OkAsync(response, cancellationToken);
    }

}
