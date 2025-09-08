using Board.Application.DTOs;
using Board.Application.Interfaces;
using MediatR;

namespace Board.Application.Queries.BoardColumns.GetBoardColumns;

public class GetBoardColumnsHandler : IRequestHandler<GetBoardColumnsQuery, List<BoardColumnDto>>
{
	private readonly IRepository<Domain.Entities.Board> _boardRepository;

	public GetBoardColumnsHandler(IRepository<Domain.Entities.Board> boardRepository)
	{
		_boardRepository = boardRepository;
	}

	public async Task<List<BoardColumnDto>> Handle(GetBoardColumnsQuery request, CancellationToken cancellationToken)
	{
		var board = await _boardRepository.GetAsync(b => b.Id == request.BoardId, cancellationToken, true, b => b.BoardColumns);
		if (board == null)
		{
			return [];
		}

		return board.BoardColumns
			.Select(c => new BoardColumnDto
			{
				Id = c.Id,
				Title = c.Title,
				Description = c.Description
			})
			.ToList();
	}
} 