using Board.Application.Commands.BoardItems.DeleteBoardItem;
using FastEndpoints;
using IMediator = MediatR.IMediator;

namespace Board.Api.Features.BoardItems.DeleteBoardItem;

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


