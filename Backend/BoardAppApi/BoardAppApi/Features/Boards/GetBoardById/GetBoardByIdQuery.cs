using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.Boards.GetBoardById;

public class GetBoardByIdQuery : IRequest<Board>
{
    public Guid Id { get; set; }
    public CancellationToken Ct { get; set; } = default(CancellationToken);
}


