using BoardAppApi.Data.Entities;
using BoardAppApi.DataStructures.Enums;
using MediatR;

namespace BoardAppApi.Features.BoardItems.CreateBoard;

public class CreateBoardItemCommand : IRequest<BoardItem>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid BoardColumnId { get; set; }
    public BoardItemPriorityEnum Priority { get; set; }
    public Guid AssigneeId { get; set; }
    public DateTime DueDate { get; set; }
}
