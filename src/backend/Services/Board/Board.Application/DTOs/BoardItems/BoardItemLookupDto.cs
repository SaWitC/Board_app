using Board.Domain.Contracts.Enums;

namespace Board.Application.DTOs.BoardItems;
public class BoardItemLookupDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid BoardColumnId { get; set; }
    public Guid BoardId { get; set; }
    public BoardItemPriorityEnum Priority { get; set; }
    public string AssigneeEmail { get; set; }
    public DateTime DueDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public DateTime CreatedTime { get; set; }
    public TaskTypeEnum TaskType { get; set; }
}
