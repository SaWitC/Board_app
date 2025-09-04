namespace Board.Application.Queries.Boards.GetBoardById;

	using Board.Application.DTOs;
	using Board.Application.Repositories;
	using MediatR;

	public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, BoardDto>
	{
		private readonly IBoardRepository _boardRepository;

		public GetBoardByIdQueryHandler(IBoardRepository boardRepository)
		{
			_boardRepository = boardRepository;
		}

		public async Task<BoardDto> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
		{
			var entity = await _boardRepository.GetAsync(request.Id, cancellationToken);
			if (entity == null)
			{
				return null;
			}

			return new BoardDto
			{
				Id = entity.Id,
				Title = entity.Title,
				Description = entity.Description,
				ModificationDate = entity.ModificationDate
			};
		}
	}
