namespace Board.Application.Repositories;

public interface IBoardRepository : IRepository<Domain.Entities.Board>
{
    Task<List<Domain.Entities.Board>> GetAllAsync(CancellationToken cancellationToken);
}
