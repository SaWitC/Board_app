using FastEndpoints;
using MediatR;
using IMediator = MediatR.IMediator;

namespace BoardAppApi.Features.BoardItems.GetBoardsItems;

public class GetBoardItemsEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boarditems");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync(await _mediator.Send(new GetBoardItemsQuery(), ct));
    }
}


