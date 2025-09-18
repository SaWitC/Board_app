using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Security;
using FastEndpoints;

namespace Board.Api.Features.BoardColumn.GetBoardColumnById;

public class GetBoardColumnByIdEndpoint : EndpointWithoutRequest
{

    private readonly IBoardColumnRepository _repository;
    private readonly MapsterMapper.IMapper _mapper;

    public GetBoardColumnByIdEndpoint(IBoardColumnRepository repository, MapsterMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Get("/api/boards/{boardId}/columns/{id}");
        Policies(Auth.BuildPermissionPolicy(Permission.Read, Context.BoardColumn, "boardId"));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        Domain.Entities.BoardColumn entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        BoardColumnDto response = entity == null
            ? null
            : _mapper.Map<BoardColumnDto>(entity);

        await Send.OkAsync(response, cancellationToken);
    }
}
