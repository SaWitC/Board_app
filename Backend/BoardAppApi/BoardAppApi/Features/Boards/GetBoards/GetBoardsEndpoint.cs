using FastEndpoints;
using IMediator = MediatR.IMediator;

namespace BoardAppApi.Features.Boards.GetBoards;

public class GetBoardsEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boards");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync(await _mediator.Send(new GetBoardsQuery(), ct));
    }
}


