namespace Board.Application.Commands.DeleteBoard;

	using MediatR;

	public class DeleteBoardCommand : IRequest<bool>
	{
		public Guid Id { get; set; }
	}
