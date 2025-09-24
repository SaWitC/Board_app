using Board.Application.Abstractions.Repositories;
using Board.Infrastructure.Data.Repositories;

namespace Board.Api.Configuration;

public static class RepositoriesConfiguration
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
        services.AddScoped<IBoardItemRepository, BoardItemRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IBoardTemplateRepository, BoardTemplateRepository>();
        services.AddScoped<IBoardUserRepository, BoardUserRepository>();

        return services;
    }
}
