using Board.Application.Commands.DeleteBoard;
using FastEndpoints;
using MediatR;
using IMediator = MediatR.IMediator;

namespace Board.Api.Features.Board.DeleteBoard;

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


