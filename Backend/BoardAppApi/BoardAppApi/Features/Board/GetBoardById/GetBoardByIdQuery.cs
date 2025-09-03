using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.Board.GetBoardById;

public class GetBoardByIdQuery : IRequest<Data.Entities.Board>
{
    public Guid Id { get; set; }
    public CancellationToken Ct { get; set; } = default(CancellationToken);
}


