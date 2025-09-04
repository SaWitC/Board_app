using FastEndpoints;
using IMediator = MediatR.IMediator;

namespace BoardAppApi.Features.Boards.DeleteBoard;

public class DeleteBoardEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/boards/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        await Send.OkAsync(await _mediator.Send(new DeleteBoardCommand { Id = id }, ct));
    }
}


