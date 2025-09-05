namespace Board.Application.Commands.DeleteBoard;

	using Board.Application.Repositories;
	using MediatR;

	public class DeleteBoardHandler : IRequestHandler<DeleteBoardCommand, bool>
	{
		private readonly IBoardRepository _boardRepository;

		public DeleteBoardHandler(IBoardRepository boardRepository)
		{
			_boardRepository = boardRepository;
		}

		public async Task<bool> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
		{
			var entity = await _boardRepository.GetAsync(request.Id, cancellationToken);
			if (entity == null)
			{
				return false;
			}

			await _boardRepository.DeleteAsync(entity, cancellationToken);
			return true;
		}
	}
