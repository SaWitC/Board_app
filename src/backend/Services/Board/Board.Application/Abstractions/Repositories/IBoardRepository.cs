using Board.Domain.Entities;

namespace Board.Application.Abstractions.Repositories;
public interface IBoardRepository : IRepository<Domain.Entities.Board>
{
    public Task<BoardUser> GetBoardOwnerAsync(Guid boardId, CancellationToken cancellationToken);
}
