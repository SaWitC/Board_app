using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.Boards.UpdateBoard;

public class UpdateBoardCommand : IRequest<Board>
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
}


