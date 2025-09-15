namespace Board.Infrastructure.Data.Repositories;
public class TagRepository : Repository<Domain.Entities.Tag>, Application.Abstractions.Repositories.ITagRepository
{
    public TagRepository(BoardDbContext context) : base(context)
    {
    }
}

