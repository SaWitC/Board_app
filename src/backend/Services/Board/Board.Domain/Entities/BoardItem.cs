using Board.Domain.Contracts.Enums;
using Board.Domain.Entities.Abstractions;

namespace Board.Domain.Entities;

public class BoardItem : IEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public BoardColumn BoardColumn { get; set; }
    public Guid BoardColumnId { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public BoardItemPriorityEnum Priority { get; set; }
    public Guid AssigneeId { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedTime { get; set; }
    public ICollection<BoardItem> SubItems { get; set; }
    public TaskTypeEnum TaskType { get; set; }
}
