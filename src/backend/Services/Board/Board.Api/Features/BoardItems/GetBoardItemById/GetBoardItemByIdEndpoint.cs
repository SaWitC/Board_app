using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Entities;
using Board.Domain.Security;
using FastEndpoints;

namespace Board.Api.Features.BoardItems.GetBoardItemById;

public class GetBoardItemByIdEndpoint : EndpointWithoutRequest
{
    private readonly IBoardItemRepository _repository;
    private readonly MapsterMapper.IMapper _mapper;

    public GetBoardItemByIdEndpoint(IBoardItemRepository repository, MapsterMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Get("/api/boards/{boardId}/items/{id}");
        Policies(Auth.BuildPermissionPolicy(Permission.Read, Context.BoardItem, "boardId"));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        BoardItem entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        if (entity == null)
        {
            await Send.OkAsync(null, cancellationToken);
        }

        BoardItemDto response = _mapper.Map<BoardItemDto>(entity);
        await Send.OkAsync(response, cancellationToken);
    }
}
