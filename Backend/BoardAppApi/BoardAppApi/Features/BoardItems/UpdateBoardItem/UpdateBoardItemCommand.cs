using BoardAppApi.Data.Entities;
using BoardAppApi.DataStructures.Enums;
using MediatR;

namespace BoardAppApi.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemCommand : IRequest<BoardItem>
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid BoardColumnId { get; set; }
    public BoardItemPriorityEnum Priority { get; set; }
    public Guid AssigneeId { get; set; }
    public DateTime DueDate { get; set; }
}


