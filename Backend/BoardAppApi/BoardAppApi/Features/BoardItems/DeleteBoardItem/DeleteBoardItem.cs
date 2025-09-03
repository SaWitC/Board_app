using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.BoardItems.DeleteBoardItems;

public class DeleteBoardItemCommand : IRequest<BoardItem>
{
    public Guid Id { get; set; }
}


