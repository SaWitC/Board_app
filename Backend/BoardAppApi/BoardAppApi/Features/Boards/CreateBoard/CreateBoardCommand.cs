using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.Boards.CreateBoard;

public class CreateBoardCommand : IRequest<Board>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
}


