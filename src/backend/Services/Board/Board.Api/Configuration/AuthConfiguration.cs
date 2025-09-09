using Board.Domain.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Board.Api.Configuration;

public static class AuthConfiguration
{
    public static IServiceCollection ConfigureAuth(this IServiceCollection services, AuthOptions authOptions)
    {
        if (authOptions.IsBypassAuthorization)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Bypass: treat any request as authenticated by injecting a fake token principal
                        if (context.Principal == null)
                        {
                            System.Security.Claims.Claim[] claims = new[]
                            {
                        new System.Security.Claims.Claim("sub", "bypass-user"),
                        new System.Security.Claims.Claim("name", "Bypass User"),
                        new System.Security.Claims.Claim("permissions", "Editor")
                    };
                            System.Security.Claims.ClaimsIdentity identity = new System.Security.Claims.ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                            context.Principal = new System.Security.Claims.ClaimsPrincipal(identity);
                        }
                        return Task.CompletedTask;
                    }
                };
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false
                };
            });
        }
        else
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
            });
        }

        //TODO: Add GLOABAL ADMIN POLICY
        services.AddAuthorization(o =>
        {
            o.AddPolicy("Editor", p => p.
                RequireAuthenticatedUser().
                RequireClaim("permissions", "Editor"));
        });

        return services;
    }
}
