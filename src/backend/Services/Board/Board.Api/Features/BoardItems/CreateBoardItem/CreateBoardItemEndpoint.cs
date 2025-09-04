using Board.Application.Commands.BoardItems.CreateBoardItem;
using FastEndpoints;
using MediatR;
using IMediator = MediatR.IMediator;

namespace Board.Api.Features.BoardItems.CreateBoardItem;

public class CreateBoardItemEndpoint(IMediator _mediator) : Endpoint<CreateBoardItemCommand>
{
    public override void Configure()
    {
        Post("/api/boarditems");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBoardItemCommand req, CancellationToken ct)
    {
        await Send.OkAsync(await _mediator.Send(req, ct));
    }
}


