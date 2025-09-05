using Board.Domain.Entities;

namespace Board.Application.Repositories;

public interface IBoardItemRepository : IRepository<BoardItem>
{
    Task<List<BoardItem>> GetAllAsync(CancellationToken cancellationToken);
}
