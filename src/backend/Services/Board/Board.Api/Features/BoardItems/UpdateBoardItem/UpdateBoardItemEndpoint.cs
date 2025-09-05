using Board.Application.Commands.BoardItems.UpdateBoardItem;
using FastEndpoints;
using MediatR;
using IMediator = MediatR.IMediator;

namespace Board.Api.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemEndpoint(IMediator _mediator) : Endpoint<UpdateBoardItemCommand>
{
    public override void Configure()
    {
        Put("/api/boarditems/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBoardItemCommand req, CancellationToken ct)
    {
        req.Id = Route<Guid>("id");
        await Send.OkAsync(await _mediator.Send(req, ct));
    }
}


