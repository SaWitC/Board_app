using FastEndpoints;
using MediatR;
using IMediator = MediatR.IMediator;

namespace BoardAppApi.Features.Board.UpdateBoard;

public class UpdateBoardEndpoint(IMediator _mediator) : Endpoint<UpdateBoardCommand>
{
    public override void Configure()
    {
        Put("/api/boards/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBoardCommand req, CancellationToken ct)
    {
        req.Id = Route<Guid>("id");
        await Send.OkAsync(await _mediator.Send(req, ct));
    }
}


