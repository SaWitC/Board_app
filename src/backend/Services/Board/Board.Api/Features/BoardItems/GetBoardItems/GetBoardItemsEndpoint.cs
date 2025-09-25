using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs.BoardItems;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Security;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Board.Api.Features.BoardItems.GetBoardItems;

[Authorize]
public class GetBoardItemsEndpoint : EndpointWithoutRequest
{
    private readonly IBoardItemRepository _repository;

    public GetBoardItemsEndpoint(IBoardItemRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/api/boards/{boardId}/items");
        Policies(Auth.BuildPermissionPolicy(Permission.ManageItems, Context.BoardItem, "boardId"));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid boardId = Route<Guid>("boardId");
        ICollection<BoardItemLookupDto> items = await _repository.GetAllBoardItemsLookup(boardId, cancellationToken);

        await Send.OkAsync(items, cancellationToken);
    }
}

