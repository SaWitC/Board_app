using Board.Application.Repositories;
using Board.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Data.Repositories.Implementations;

public class BoardRepository : BaseRepository<Domain.Entities.Board>, IBoardRepository
{
    public BoardRepository(BoardDbContext context) : base(context)
    {
    }

    public async Task<List<Domain.Entities.Board>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Set<Domain.Entities.Board>().ToListAsync(cancellationToken);
    }
}
