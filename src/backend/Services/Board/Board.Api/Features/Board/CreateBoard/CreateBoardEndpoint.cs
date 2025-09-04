using FastEndpoints;
using MediatR;
using Board.Application.Commands.CreateBoard;

namespace Board.Api.Features.Board.CreateBoard;

public class CreateBoardEndpoint(IMediator _mediator) : Endpoint<CreateBoardCommand>
{
    public override void Configure()
    {
        Post("/api/boards");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBoardCommand req, CancellationToken ct)
    {
        await Send.OkAsync(await _mediator.Send(req, ct));
    }
}
