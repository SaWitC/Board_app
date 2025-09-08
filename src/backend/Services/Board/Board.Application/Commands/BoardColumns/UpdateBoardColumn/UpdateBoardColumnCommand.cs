namespace Board.Application.Commands.BoardColumns.UpdateBoardColumn;

	using MediatR;
	using Board.Application.DTOs;

	public class UpdateBoardColumnCommand : IRequest<BoardColumnDto>
	{
		public Guid Id { get; set; }
		public required string Title { get; set; }
		public required string Description { get; set; }
	} 