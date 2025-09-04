namespace Board.Application.Commands.UpdateBoard;

	using Board.Application.DTOs;
	using Board.Application.Repositories;
	using MediatR;

	public class UpdateBoardHandler : IRequestHandler<UpdateBoardCommand, BoardDto>
	{
		private readonly IBoardRepository _boardRepository;

		public UpdateBoardHandler(IBoardRepository boardRepository)
		{
			_boardRepository = boardRepository;
		}

		public async Task<BoardDto> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
		{
			var entity = await _boardRepository.GetAsync(request.Id, cancellationToken);
			if (entity == null)
			{
				return null;
			}

			entity.Title = request.Title;
			entity.Description = request.Description;
			entity.ModificationDate = DateTimeOffset.UtcNow;

			var updated = await _boardRepository.UpdateAsync(entity, cancellationToken);
			return new BoardDto
			{
				Id = updated.Id,
				Title = updated.Title,
				Description = updated.Description,
				ModificationDate = updated.ModificationDate
			};
		}
	}
