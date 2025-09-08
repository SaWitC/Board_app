using Board.Application.Commands.BoardColumns.DeleteBoardColumn;
using FastEndpoints;
using IMediator = MediatR.IMediator;

namespace Board.Api.Features.BoardColumn.DeleteBoardColumn;

public class DeleteBoardColumnEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
	public override void Configure()
	{
		Delete("/api/boards/{boardId}/columns/{id}");
		AllowAnonymous();
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		Guid id = Route<Guid>("id");
		await Send.OkAsync(await _mediator.Send(new DeleteBoardColumnCommand { Id = id }, ct), ct);
	}
} 
