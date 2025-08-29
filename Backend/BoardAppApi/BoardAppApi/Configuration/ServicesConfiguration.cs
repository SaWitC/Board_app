using BoardAppApi.Data.Repositories.Abstractions;

namespace BoardAppApi.Configuration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public static IServiceCollection ConfigureRepositories(IServiceCollection services,IConfiguration configuration)
        {

            services.Scan(selector => selector
                    .FromEntryAssembly()
                    .AddClasses(classSelector =>
                    classSelector.InNamespaces("BoardAppApi.Data.Repositories.Implemntations"))
                .AsMatchingInterface());
            return services;
        }
    }
}
