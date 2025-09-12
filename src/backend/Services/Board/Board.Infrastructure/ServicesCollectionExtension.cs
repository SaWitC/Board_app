using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Board.Infrastructure;
public static class ServicesCollectionExtension
{

    //public static IServiceCollection AddHrm(this IServiceCollection services, IConfiguration configuration)
    //{
    //    services.AddRefitClient<IAuthApiClient>()
    //        .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration?.GetValue<string>("HRM:KeycloakUrl") ?? string.Empty));

    //    services.AddRefitClient<IEmployeeApiClient>()
    //        .ConfigureHttpClient((provider, client) =>
    //        {
    //            IAuthApiClient authClient = provider.GetRequiredService<IAuthApiClient>();
    //            KeyCloakTokenModel tokenResponse = authClient.GetTokenAsync(new KeyCloakTokenRequestParameters()
    //            {
    //                ClientId = configuration.GetValue<string>("HRM:CLIENT_ID")!,
    //                Username = configuration.GetValue<string>("HRM:HRM_USERNAME")!,
    //                Password = configuration.GetValue<string>("HRM:HRM_PASSWORD")!,
    //            }).Result;

    //            client.BaseAddress = new Uri(configuration.GetValue<string>("HRM:EmployeeApiUrl") ?? string.Empty);
    //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
    //        });

    //    return services;
    //}

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddHrm(configuration);

        return services;
    }
}
