using Board.Application.Abstractions.Repositories;
using Board.Domain;
using Board.Infrastructure.Data.Repositories;

namespace Board.Infrastructure.Data.UoW;

public class UnitOfWork : BaseDisposable, IUnitOfWork
{
    private Lazy<IBoardRepository> _boards;
    private Lazy<IBoardItemRepository> _boardItems;
    private Lazy<IBoardColumnRepository> _boardColumns;
    private Lazy<IBoardTemplateRepository> _boardTemplates;
    private Lazy<ITagRepository> _tags;

    public IBoardRepository Boards => (_boards ??= new Lazy<IBoardRepository>(() => new BoardRepository(Context))).Value;
    public IBoardItemRepository BoardItems => (_boardItems ??= new Lazy<IBoardItemRepository>(() => new BoardItemRepository(Context))).Value;
    public IBoardColumnRepository BoardColumns => (_boardColumns ??= new Lazy<IBoardColumnRepository>(() => new BoardColumnRepository(Context))).Value;
    public IBoardTemplateRepository BoardTemplates => (_boardTemplates ??= new Lazy<IBoardTemplateRepository>(() => new BoardTemplateRepository(Context))).Value;
    public ITagRepository Tags => (_tags ??= new Lazy<ITagRepository>(() => new TagRepository(Context))).Value;


    public BoardDbContext Context { get; }

    public UnitOfWork(BoardDbContext dbContext)
    {
        Context = dbContext;
    }

    public Task<int> SaveChangesAsync()
    {
        return Context.SaveChangesAsync();
    }

    protected override void DisposeManagedResources()
    {
        Context?.Dispose();
    }
}
