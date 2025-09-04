using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.Boards.DeleteBoard;

public class DeleteBoardCommand : IRequest<Board>
{
    public Guid Id { get; set; }
}


