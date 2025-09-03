using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.Board.UpdateBoard;

public class UpdateBoardCommand : IRequest<Data.Entities.Board>
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
}


