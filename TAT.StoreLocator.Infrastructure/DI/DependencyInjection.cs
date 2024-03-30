using Microsoft.Extensions.DependencyInjection;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Infrastructure.Services;

namespace TAT.StoreLocator.Infrastructure.DI
{
    public static class DependencyInjection

    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            #region Services
            _ = services.AddSingleton<IUserService, UserService>();
            #endregion

            #region Repositories

            #endregion


            return services;
        }

    }
}
