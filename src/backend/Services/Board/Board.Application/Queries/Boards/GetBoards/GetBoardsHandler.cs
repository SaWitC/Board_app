namespace Board.Application.Queries.Boards.GetBoards;

	using Board.Application.DTOs;
	using Board.Application.Repositories;
	using MediatR;

	public class GetBoardsHandler : IRequestHandler<GetBoardsQuery, List<BoardDto>>
	{
		private readonly IBoardRepository _boardRepository;

		public GetBoardsHandler(IBoardRepository boardRepository)
		{
			_boardRepository = boardRepository;
		}

		public async Task<List<BoardDto>> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
		{
			var boards = await _boardRepository.GetAllAsync(cancellationToken);
			return boards.Select(b => new BoardDto
			{
				Id = b.Id,
				Title = b.Title,
				Description = b.Description,
				ModificationDate = b.ModificationDate
			}).ToList();
		}
	}
