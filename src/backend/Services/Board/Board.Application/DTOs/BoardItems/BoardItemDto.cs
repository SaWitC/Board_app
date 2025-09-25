namespace Board.Application.DTOs.BoardItems;

using Board.Application.DTOs.Tags;
using Board.Domain.Contracts.Enums;

public class BoardItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid BoardColumnId { get; set; }
    public BoardItemPriorityEnum Priority { get; set; }
    public string AssigneeEmail { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public TaskTypeEnum TaskType { get; set; }
    public List<TagDto> Tags { get; set; }
}
