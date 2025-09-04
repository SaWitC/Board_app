namespace Board.Application.Commands.BoardItems.DeleteBoardItem;

	using MediatR;

	public class DeleteBoardItemCommand : IRequest<bool>
	{
		public Guid Id { get; set; }
	}
