using Board.Application.Commands.BoardColumns.CreateBoardColumn;
using FastEndpoints;
using IMediator = MediatR.IMediator;

namespace Board.Api.Features.BoardColumn.CreateBoardColumn;

public class CreateBoardColumnEndpoint(IMediator _mediator) : Endpoint<CreateBoardColumnCommand>
{
	public override void Configure()
	{
		Post("/api/boards/{boardId}/columns");
		AllowAnonymous();
	}

	public override async Task HandleAsync(CreateBoardColumnCommand req, CancellationToken ct)
	{
		req.BoardId = Route<Guid>("boardId");
		await Send.OkAsync(await _mediator.Send(req, ct), ct);
	}
} 
