using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.BoardItems.GetBoardById;

public class GetBoardItemByIdQuery : IRequest<BoardItem>
{
    public Guid Id { get; set; }
    public CancellationToken Ct { get; set; } = default(CancellationToken);
}
