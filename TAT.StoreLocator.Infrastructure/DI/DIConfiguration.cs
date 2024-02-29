using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TAT.StoreLocator.Infrastructure.DI
{
    public static class DIConfiguration
    {

        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

    }
}
