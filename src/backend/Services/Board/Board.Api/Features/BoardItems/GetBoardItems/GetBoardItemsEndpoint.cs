using Board.Application.Abstractions.Repositories;
using Board.Application.Abstractions.Services;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Entities;
using Board.Domain.Security;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Board.Api.Features.BoardItems.GetBoardItems;

[Authorize]
public class GetBoardItemsEndpoint : EndpointWithoutRequest
{
    private readonly IBoardItemRepository _repository;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly MapsterMapper.IMapper _mapper;

    public GetBoardItemsEndpoint(IBoardItemRepository repository, MapsterMapper.IMapper mapper, ICurrentUserProvider currentUserProvider)
    {
        _repository = repository;
        _currentUserProvider = currentUserProvider;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/api/boards/{boardId}/items");
        Policies(Auth.BuildPermissionPolicy(Permission.ManageItems, Context.BoardItem, "boardId"));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        string email = _currentUserProvider.GetUserEmail();
        IList<BoardItem> entities = await _repository.GetAllAsync(
            predicate: i => i.BoardColumn.Board.BoardUsers.Any(u => u.Email == email),
            cancellationToken: cancellationToken,
            asNoTracking: true,
            i => i.BoardColumn,
            i => i.BoardColumn.Board,
            i => i.BoardColumn.Board.BoardUsers);
        IList<BoardItemDto> items = _mapper.Map<IList<BoardItemDto>>(entities);
        await Send.OkAsync(items, cancellationToken);
    }
}

