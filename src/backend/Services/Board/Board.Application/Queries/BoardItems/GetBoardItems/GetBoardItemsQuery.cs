namespace Board.Application.Queries.BoardItems.GetBoardItems;

	using Board.Application.DTOs;
	using MediatR;

	public class GetBoardItemsQuery : IRequest<List<BoardItemDto>>
	{
	}
