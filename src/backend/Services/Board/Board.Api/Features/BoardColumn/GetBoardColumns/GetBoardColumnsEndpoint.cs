using Board.Application.Queries.BoardColumns.GetBoardColumns;
using FastEndpoints;
using MediatR;

namespace Board.Api.Features.BoardColumn.GetBoardColumns;

public class GetBoardColumnsEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
	public override void Configure()
	{
		Get("/api/boards/{boardId}/columns");
		AllowAnonymous();
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
        Guid boardId = Route<Guid>("boardId");
        await Send.OkAsync(await _mediator.Send(new GetBoardColumnsQuery { BoardId = boardId }, ct), ct);
	}
} 
