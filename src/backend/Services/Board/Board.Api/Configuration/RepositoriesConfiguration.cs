using Board.Application.Interfaces;
using Board.Domain.Entities;
using Board.Infrastructure.Data.Repositories;

namespace Board.Api.Configuration;

public static class RepositoriesConfiguration
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRepository<Domain.Entities.Board>, Repository<Domain.Entities.Board>>();
        services.AddScoped<IRepository<BoardItem>, Repository<BoardItem>>();
        services.AddScoped<IRepository<BoardColumn>, Repository<BoardColumn>>();
        services.AddScoped<IRepository<Tag>, Repository<Tag>>();
        services.AddScoped<IRepository<BoardTemplate>, Repository<BoardTemplate>>();

        return services;
    }
}
