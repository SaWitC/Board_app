using FastEndpoints;
using IMediator = MediatR.IMediator;

namespace BoardAppApi.Features.BoardItems.GetBoardById;

public class GetBoardItemByIdEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boarditems/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        await Send.OkAsync(_mediator.Send(new GetBoardItemByIdQuery { Id = id, Ct = ct }));
    }
}
