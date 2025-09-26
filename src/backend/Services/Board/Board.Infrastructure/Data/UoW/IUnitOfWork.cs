using Board.Application.Abstractions.Repositories;
namespace Board.Infrastructure.Data.UoW;

public interface IUnitOfWork : IDisposable
{
    IBoardRepository Boards { get; }
    IBoardItemRepository BoardItems { get; }
    IBoardColumnRepository BoardColumns { get; }
    IBoardTemplateRepository BoardTemplates { get; }
    ITagRepository Tags { get; }

    BoardDbContext Context { get; }

    Task<int> SaveChangesAsync();
}
