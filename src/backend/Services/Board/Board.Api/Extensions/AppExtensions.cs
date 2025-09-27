using Board.Infrastructure;

namespace Board.Api.Extensions;

public static class AppExtensions
{
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            DatabaseInitializer.InitializeDatabase(scope.ServiceProvider);
        }

        return app;
    }
}
