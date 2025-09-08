namespace Board.Application.Queries.BoardColumns.GetBoardColumns;

	using Board.Application.DTOs;
	using MediatR;

	public class GetBoardColumnsQuery : IRequest<List<BoardColumnDto>>
	{
		public Guid BoardId { get; set; }
	} 