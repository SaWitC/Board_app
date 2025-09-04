namespace Board.Application.Queries.Boards.GetBoardById;

	using Board.Application.DTOs;
	using MediatR;

	public class GetBoardByIdQuery : IRequest<BoardDto>
	{
		public Guid Id { get; set; }
	}
