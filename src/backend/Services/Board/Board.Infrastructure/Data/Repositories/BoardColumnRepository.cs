using Board.Application.Abstractions.Repositories;
using Board.Domain.Entities;

namespace Board.Infrastructure.Data.Repositories;
public class BoardColumnRepository : Repository<BoardColumn>, IBoardColumnRepository
{
    public BoardColumnRepository(BoardDbContext context) : base(context)
    {
    }
}
