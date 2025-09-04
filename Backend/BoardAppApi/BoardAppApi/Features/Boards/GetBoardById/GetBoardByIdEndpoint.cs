using FastEndpoints;
using IMediator = MediatR.IMediator;

namespace BoardAppApi.Features.Boards.GetBoardById;

public class GetBoardByIdEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boards/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        await Send.OkAsync(_mediator.Send(new GetBoardByIdQuery { Id = id, Ct = ct }));
    }
}


