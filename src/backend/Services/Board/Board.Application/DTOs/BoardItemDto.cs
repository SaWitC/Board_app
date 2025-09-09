namespace Board.Application.DTOs;

using Board.Domain.Contracts.Enums;

public class BoardItemDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public Guid BoardColumnId { get; set; }
		public BoardItemPriorityEnum Priority { get; set; }
		public Guid AssigneeId { get; set; }
		public DateTime DueDate { get; set; }
		public DateTimeOffset ModificationDate { get; set; }
		public DateTime CreatedTime { get; set; }
	}
