namespace Board.Application.Queries.Boards.GetBoards;

	using Board.Application.DTOs;
	using MediatR;

	public class GetBoardsQuery : IRequest<List<BoardDto>>
	{
	}
