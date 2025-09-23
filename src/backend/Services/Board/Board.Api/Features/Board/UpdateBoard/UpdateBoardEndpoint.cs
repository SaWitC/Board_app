using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Security;
using Board.Infrastructure.Data.Extensions;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.Board.UpdateBoard;

public class UpdateBoardEndpoint : Endpoint<UpdateBoardRequest>
{
    private readonly IBoardRepository _repository;
    private readonly IMapper _mapper;
    public UpdateBoardEndpoint(IBoardRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put("/api/boards/{boardId}");
        Policies(Auth.BuildPermissionPolicy(Permission.ManageBoard, Context.Board, "boardId"));
    }

    public override async Task HandleAsync(UpdateBoardRequest request, CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("boardId");

        Domain.Entities.Board entity = await _repository.GetAsync(x => x.Id == id, cancellationToken, false, x => x.BoardColumns, x => x.BoardUsers);
        if (entity == null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        //set columns order
        int order = 0;
        foreach (BoardColumnDto column in request.BoardColumns)
        {
            column.Order = order;
            order++;
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.ModificationDate = DateTimeOffset.UtcNow;
        entity.BoardColumns.Synchronize(request.BoardColumns,
            entityColumn => entityColumn.Id,
            requestColumn => requestColumn.Id,
            updateAction: (entityColumn, requestColumn) =>
            {
                entityColumn.Title = requestColumn.Title;
                entityColumn.Order = requestColumn.Order;
                entityColumn.Description = requestColumn.Description;
            },
            createAction: requestColumn => new Domain.Entities.BoardColumn
            {
                Id = Guid.NewGuid(),
                Title = requestColumn.Title,
                Description = requestColumn.Description,
                Elements = [],
                Order = requestColumn.Order

            }
        );
        entity.BoardUsers.Synchronize(request.BoardUsers,
            entityUser => entityUser.Email,
            requestUser => requestUser.Email,
            updateAction: (entityUser, requestUser) =>
            {
                entityUser.Role = requestUser.Role;
            },
            createAction: requestUser => new Domain.Entities.BoardUser
            {
                BoardId = id,
                Email = requestUser.Email,
                Role = requestUser.Role
            },
            StringComparer.OrdinalIgnoreCase
        );

        Domain.Entities.Board updated = await _repository.UpdateAsync(entity, cancellationToken);
        BoardDto response = _mapper.Map<BoardDto>(updated);

        await Send.OkAsync(response, cancellationToken);
    }
}

