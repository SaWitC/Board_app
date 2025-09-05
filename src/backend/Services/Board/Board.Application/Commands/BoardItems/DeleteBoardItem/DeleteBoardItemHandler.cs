namespace Board.Application.Commands.BoardItems.DeleteBoardItem;

	using Board.Application.Repositories;
	using MediatR;

	public class DeleteBoardItemHandler : IRequestHandler<DeleteBoardItemCommand, bool>
	{
		private readonly IBoardItemRepository _repository;

		public DeleteBoardItemHandler(IBoardItemRepository repository)
		{
			_repository = repository;
		}

		public async Task<bool> Handle(DeleteBoardItemCommand request, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetAsync(request.Id, cancellationToken);
			if (entity == null)
			{
				return false;
			}

			await _repository.DeleteAsync(entity, cancellationToken);
			return true;
		}
	}
