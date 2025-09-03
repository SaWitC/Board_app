using FastEndpoints;
using MediatR;
using IMediator = MediatR.IMediator;

namespace BoardAppApi.Features.BoardItems.DeleteBoardItems;

public class DeleteBoardItemEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/boarditems/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        await Send.OkAsync(await _mediator.Send(new DeleteBoardItemCommand { Id = id }, ct));
    }
}


