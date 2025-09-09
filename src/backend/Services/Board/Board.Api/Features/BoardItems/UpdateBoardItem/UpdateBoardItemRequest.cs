using Board.Domain.Contracts.Enums;

namespace Board.Api.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid BoardColumnId { get; set; }
    public BoardItemPriorityEnum Priority { get; set; }
    public Guid AssigneeId { get; set; }
    public DateTime DueDate { get; set; }
}
