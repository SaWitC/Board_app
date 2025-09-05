namespace Board.Application.Commands.UpdateBoard;

	using Board.Application.DTOs;
	using MediatR;

	public class UpdateBoardCommand : IRequest<BoardDto>
	{
		public Guid Id { get; set; }
		public required string Title { get; set; }
		public required string Description { get; set; }
	}
