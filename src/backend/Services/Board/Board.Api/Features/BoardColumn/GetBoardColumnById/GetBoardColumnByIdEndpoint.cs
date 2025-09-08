using Board.Application.Queries.BoardColumns.GetBoardColumnById;
using FastEndpoints;
using IMediator = MediatR.IMediator;

namespace Board.Api.Features.BoardColumn.GetBoardColumnById;

public class GetBoardColumnByIdEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
	public override void Configure()
	{
		Get("/api/boards/{boardId}/columns/{id}");
		AllowAnonymous();
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		Guid id = Route<Guid>("id");
		await Send.OkAsync(await _mediator.Send(new GetBoardColumnByIdQuery { Id = id }, ct), ct);
	}
} 
