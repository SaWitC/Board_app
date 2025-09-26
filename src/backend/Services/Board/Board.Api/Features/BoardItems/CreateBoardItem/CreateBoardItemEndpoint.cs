using Board.Application.DTOs.BoardItems;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Entities;
using Board.Domain.Security;
using Board.Infrastructure.Data.UoW;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardItems.CreateBoardItem;

public class CreateBoardItemEndpoint : Endpoint<CreateBoardItemRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBoardItemEndpoint(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Post("/api/boards/{boardId}/columns/{boardColumnId}/items");
        Policies(Auth.BuildPermissionPolicy(Permission.ManageItems, Context.BoardItem, "boardId"));
    }

    public override async Task HandleAsync(CreateBoardItemRequest request, CancellationToken cancellationToken)
    {
        Guid boardId = Route<Guid>("boardId");
        var tagsForEntity = await _unitOfWork.Tags.GetOrCreateTagsAsync(request.Tags, cancellationToken);
        BoardItem entity = new()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            BoardColumnId = request.BoardColumnId,
            BoardId = boardId,
            Priority = request.Priority,
            AssigneeEmail = request.AssigneeEmail,
            DueDate = request.DueDate,
            CreatedTime = DateTime.UtcNow,
            ModificationDate = DateTimeOffset.UtcNow,
            TaskType = request.TaskType,
            Tags = tagsForEntity,
        };

        BoardItem created = await _unitOfWork.BoardItems.AddAsync(entity, cancellationToken);
        BoardItemDto response = _mapper.Map<BoardItemDto>(created);

        await Send.OkAsync(response, cancellationToken);
    }
}


