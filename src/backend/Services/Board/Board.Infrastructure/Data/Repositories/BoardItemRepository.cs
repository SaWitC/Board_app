using Board.Application.Abstractions.Repositories;
using Board.Domain.Entities;

namespace Board.Infrastructure.Data.Repositories;
public class BoardItemRepository : Repository<BoardItem>, IBoardItemRepository
{
    public BoardItemRepository(BoardDbContext context) : base(context)
    {
    }
}
