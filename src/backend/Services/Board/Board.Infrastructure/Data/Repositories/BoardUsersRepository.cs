using Board.Application.Abstractions.Repositories;
using Board.Domain.Entities;

namespace Board.Infrastructure.Data.Repositories;
public class BoardUsersRepository : Repository<BoardUser>, IBoardUsersRepository
{
    public BoardUsersRepository(BoardDbContext context) : base(context)
    {
    }
}
