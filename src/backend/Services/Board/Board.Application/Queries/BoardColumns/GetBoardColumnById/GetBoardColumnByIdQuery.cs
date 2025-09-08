namespace Board.Application.Queries.BoardColumns.GetBoardColumnById;

	using Board.Application.DTOs;
	using MediatR;

	public class GetBoardColumnByIdQuery : IRequest<BoardColumnDto>
	{
		public Guid Id { get; set; }
	} 
