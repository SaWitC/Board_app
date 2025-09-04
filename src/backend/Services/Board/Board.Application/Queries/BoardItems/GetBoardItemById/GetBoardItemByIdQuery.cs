namespace Board.Application.Queries.BoardItems.GetBoardItemById;

	using Board.Application.DTOs;
	using MediatR;

	public class GetBoardItemByIdQuery : IRequest<BoardItemDto>
	{
		public Guid Id { get; set; }
	}
