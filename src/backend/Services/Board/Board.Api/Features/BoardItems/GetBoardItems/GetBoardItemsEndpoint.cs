using Board.Application.Queries.BoardItems.GetBoardItems;
using FastEndpoints;
using MediatR;

namespace Board.Api.Features.BoardItems.GetBoardItems;

public class GetBoardItemsEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boarditems");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync(await _mediator.Send(new GetBoardItemsQuery(), ct), ct);
    }
}


