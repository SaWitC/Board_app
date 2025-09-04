using FastEndpoints;
using IMediator = MediatR.IMediator;

namespace BoardAppApi.Features.BoardItems.CreateBoard;

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


