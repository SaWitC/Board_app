using Board.Application.Abstractions.Repositories;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Security;
using FastEndpoints;

namespace Board.Api.Features.Board.DeleteBoard;

public class DeleteBoardEndpoint : EndpointWithoutRequest
{
	public override void Configure()
	{
		Delete("/api/boards/{boardId}");
		Policies(Auth.BuildPermissionPolicy(Permission.ManageBoard, Context.BoardColumn, "boardId"));
	}

	private readonly IBoardRepository _repository;
	public DeleteBoardEndpoint(IBoardRepository repository)
	{
		_repository = repository;
	}

	public override async Task HandleAsync(CancellationToken cancellationToken)
	{
		Guid id = Route<Guid>("boardId");

		Domain.Entities.Board entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
		if (entity == null)
		{
			await Send.NotFoundAsync(cancellationToken);
			return;
		}
		await _repository.DeleteAsync(entity, cancellationToken);

		await Send.NoContentAsync(cancellationToken);
	}
}
