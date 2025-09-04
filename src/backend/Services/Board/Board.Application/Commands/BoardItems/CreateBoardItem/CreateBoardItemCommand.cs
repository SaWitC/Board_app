namespace Board.Application.Commands.BoardItems.CreateBoardItem;

	using Board.Application.DTOs;
	using Board.Domain.Enums;
	using MediatR;

	public class CreateBoardItemCommand : IRequest<BoardItemDto>
	{
		public required string Title { get; set; }
		public required string Description { get; set; }
		public Guid BoardColumnId { get; set; }
		public BoardItemPriorityEnum Priority { get; set; }
		public Guid AssigneeId { get; set; }
		public DateTime DueDate { get; set; }
	}
