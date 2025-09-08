namespace Board.Api.Configuration;

public static class ConfigureCors
{
    public static IServiceCollection ConfigureAuth(this IServiceCollection services, string defaultPolicyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(defaultPolicyName, policy =>
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );
        });

        return services;
    }
}
