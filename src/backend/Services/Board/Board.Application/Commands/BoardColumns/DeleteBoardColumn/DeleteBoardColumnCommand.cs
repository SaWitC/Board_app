namespace Board.Application.Commands.BoardColumns.DeleteBoardColumn;

	using MediatR;

	public class DeleteBoardColumnCommand : IRequest<bool>
	{
		public Guid Id { get; set; }
	} 