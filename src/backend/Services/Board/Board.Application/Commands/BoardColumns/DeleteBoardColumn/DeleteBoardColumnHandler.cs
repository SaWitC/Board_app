using Board.Application.Interfaces;
using Board.Domain.Entities;
using MediatR;

namespace Board.Application.Commands.BoardColumns.DeleteBoardColumn;

public class DeleteBoardColumnHandler : IRequestHandler<DeleteBoardColumnCommand, bool>
{
	private readonly IRepository<BoardColumn> _repository;

	public DeleteBoardColumnHandler(IRepository<BoardColumn> repository)
	{
		_repository = repository;
	}

	public async Task<bool> Handle(DeleteBoardColumnCommand request, CancellationToken cancellationToken)
	{
		var entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken, false);
		if (entity == null)
		{
			return false;
		}

		await _repository.DeleteAsync(entity, cancellationToken);
		return true;
	}
} 