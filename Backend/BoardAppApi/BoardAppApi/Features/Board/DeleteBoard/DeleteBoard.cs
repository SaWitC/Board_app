using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.Board.DeleteBoard;

public class DeleteBoardCommand : IRequest<Data.Entities.Board>
{
    public Guid Id { get; set; }
}


