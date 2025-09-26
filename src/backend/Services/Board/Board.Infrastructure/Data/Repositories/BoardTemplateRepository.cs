namespace Board.Infrastructure.Data.Repositories;
public class BoardTemplateRepository : Repository<Domain.Entities.BoardTemplate>, Application.Abstractions.Repositories.IBoardTemplateRepository
{
    public BoardTemplateRepository(BoardDbContext context) : base(context)
    {
    }
}
