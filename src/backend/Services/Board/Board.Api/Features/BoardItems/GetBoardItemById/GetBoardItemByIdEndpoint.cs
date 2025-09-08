using Board.Application.Queries.BoardItems.GetBoardItemById;
using FastEndpoints;
using MediatR;

namespace Board.Api.Features.BoardItems.GetBoardItemById;

public class GetBoardItemByIdEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boarditems/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        await Send.OkAsync(await _mediator.Send(new GetBoardItemByIdQuery { Id = id }, ct), ct);
    }
}
