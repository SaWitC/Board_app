using Board.Domain.Contracts.Enums;
using FastEndpoints;

namespace Board.Api.Features.BoardItems.CreateBoardItem;

public class CreateBoardItemRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    [QueryParam]
    public Guid BoardColumnId { get; set; }
    public BoardItemPriorityEnum Priority { get; set; }
    public Guid AssigneeId { get; set; }
    public DateTime DueDate { get; set; }
    public TaskTypeEnum TaskType { get; set; }

}
