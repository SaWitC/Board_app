using FastEndpoints.Swagger;

namespace Board.Api.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen();

        services.SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "My API";
                s.Version = "v1";
            };
        });

        return services;
    }
}
