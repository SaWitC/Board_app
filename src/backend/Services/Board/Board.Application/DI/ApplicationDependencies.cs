using Board.Application.Abstractions.Services;
using Board.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Board.Application.DI;
public static class ApplicationDependencies
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection service)
    {
        service.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return service;
    }
}
