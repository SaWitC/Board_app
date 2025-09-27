using Board.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Board.Infrastructure;

public static class DatabaseInitializer
{
    public static void InitializeDatabase(IServiceProvider provider)
    {
        var context = provider.GetRequiredService<BoardDbContext>();
        context.Database.Migrate();
    }
}
