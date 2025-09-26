using Board.Application.Abstractions.Repositories;
using Board.Domain.Entities;

namespace Board.Infrastructure.Data.Repositories;

public class BoardUserRepository : Repository<BoardUser>, IBoardUserRepository
{
    public BoardUserRepository(BoardDbContext context) : base(context)
    {
    }
}
