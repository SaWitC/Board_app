using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Board.ServiceDefaults;

public static class DatabaseExtension
{
    public static IServiceCollection AddDatabase<TContext, TOptions>(
        this IHostApplicationBuilder builder,
        Func<TOptions, string> connectionStringSelector,
        string assemblyName = null,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TContext : DbContext
        where TOptions : class
    {
        builder.Services.AddDatabaseWithoutEnrich<TContext, TOptions>(connectionStringSelector, assemblyName, serviceLifetime);

        return builder.Services;
    }

    public static IServiceCollection AddDatabaseWithoutEnrich<TContext, TOptions>(
        this IServiceCollection services,
        Func<TOptions, string> connectionStringSelector,
        string assemblyName = null,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TContext : DbContext
        where TOptions : class
    {
        services.AddDbContext<TContext>((serviceProvider, optionsBuilder) =>
        {
            TOptions options = serviceProvider.GetRequiredService<IOptions<TOptions>>().Value;
            string connectionString = connectionStringSelector(options);

            optionsBuilder.UseNpgsql(connectionString, o =>
            {
                o.EnableRetryOnFailure();
                if (!string.IsNullOrWhiteSpace(assemblyName))
                {
                    o.MigrationsAssembly(assemblyName);
                }
            });
        },
        serviceLifetime);

        return services;
    }
}
