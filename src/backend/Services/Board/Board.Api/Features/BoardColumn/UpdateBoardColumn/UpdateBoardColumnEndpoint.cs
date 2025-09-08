using Board.Application.Commands.BoardColumns.UpdateBoardColumn;
using FastEndpoints;
using IMediator = MediatR.IMediator;

namespace Board.Api.Features.BoardColumn.UpdateBoardColumn;

public class UpdateBoardColumnEndpoint(IMediator _mediator) : Endpoint<UpdateBoardColumnCommand>
{
	public override void Configure()
	{
		Put("/api/boards/{boardId}/columns");
		AllowAnonymous();
	}

	public override async Task HandleAsync(UpdateBoardColumnCommand req, CancellationToken ct)
	{
		await Send.OkAsync(await _mediator.Send(req, ct), ct);
	}
} 
