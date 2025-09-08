using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using MediatR;

namespace Board.Application.Commands.BoardColumns.CreateBoardColumn;

public class CreateBoardColumnHandler : IRequestHandler<CreateBoardColumnCommand, BoardColumnDto>
{
	private readonly IRepository<BoardColumn> _columnRepository;
	private readonly IRepository<Domain.Entities.Board> _boardRepository;

	public CreateBoardColumnHandler(IRepository<BoardColumn> columnRepository, IRepository<Domain.Entities.Board> boardRepository)
	{
		_columnRepository = columnRepository;
		_boardRepository = boardRepository;
	}

	public async Task<BoardColumnDto> Handle(CreateBoardColumnCommand request, CancellationToken cancellationToken)
	{
		var board = await _boardRepository.GetAsync(b => b.Id == request.BoardId, cancellationToken, true, b => b.BoardColumns);
		if (board == null)
		{
			throw new InvalidOperationException("Board not found");
		}

		var entity = new BoardColumn
		{
			Id = Guid.NewGuid(),
			Title = request.Title,
			Description = request.Description
		};

		board.BoardColumns.Add(entity);
		await _columnRepository.AddAsync(entity, cancellationToken);
		await _boardRepository.UpdateAsync(board, cancellationToken);

		return new BoardColumnDto
		{
			Id = entity.Id,
			Title = entity.Title,
			Description = entity.Description
		};
	}
} 
