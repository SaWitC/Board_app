using Board.Domain.Entities;

namespace Board.Application.Abstractions.Repositories;
public interface IBoardColumnRepository : IRepository<BoardColumn>
{
    public Task<IList<BoardColumn>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
}
