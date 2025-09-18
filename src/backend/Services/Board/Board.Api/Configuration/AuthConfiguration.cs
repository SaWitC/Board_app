using Board.Domain.Options;
using Board.Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Board.Api.Configuration;

public static class AuthConfiguration
{
    public static IServiceCollection ConfigureAuth(this IServiceCollection services, AuthOptions authOptions)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = authOptions.Authority;
            options.Audience = authOptions.Audience;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                RoleClaimType = Auth.Claims.Permissions
            };
        });

        services.AddAuthorizationBuilder()
                .AddPolicy(Auth.Policies.GlobalAdminPolicy, p => p
                    .RequireAuthenticatedUser()
                    .RequireClaim(Auth.Claims.Permissions, Auth.Roles.GlobalAdmin));

        return services;
    }
}
