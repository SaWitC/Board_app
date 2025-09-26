using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Security;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardColumn.UpdateBoardColumn;

public class UpdateBoardColumnEndpoint : Endpoint<UpdateBoardColumnRequest>
{
    private readonly IBoardColumnRepository _repository;
    private readonly IMapper _mapper;

    public UpdateBoardColumnEndpoint(IBoardColumnRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put("/api/boards/{boardId}/columns");
        Policies(Auth.BuildPermissionPolicy(Permission.ManageBoard, Context.BoardColumn, "boardId"));
    }

    public override async Task HandleAsync(UpdateBoardColumnRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.BoardColumn entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken, false)
            ?? throw new InvalidOperationException("Column not found");
        entity.Title = request.Title;
        entity.Description = request.Description;

        Domain.Entities.BoardColumn updated = await _repository.UpdateAsync(entity, cancellationToken);

        BoardColumnDto response = _mapper.Map<BoardColumnDto>(updated);
        await Send.OkAsync(response, cancellationToken);
    }
}
