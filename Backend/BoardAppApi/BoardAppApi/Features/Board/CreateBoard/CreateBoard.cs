using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.Board.CreateBoard;

public class CreateBoardCommand : IRequest<Data.Entities.Board>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
}


