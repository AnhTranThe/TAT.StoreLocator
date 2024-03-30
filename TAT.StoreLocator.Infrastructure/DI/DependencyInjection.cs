using Microsoft.Extensions.DependencyInjection;
using TAT.StoreLocator.Core.Interface.IRepositories;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Interface.IUnitOfWork;
using TAT.StoreLocator.Infrastructure.Repositories;
using TAT.StoreLocator.Infrastructure.Services;
using TAT.StoreLocator.Infrastructure.UnitOfWork;


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
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IUnitOfWork, UnitWork>();
            #endregion


            return services;
        }

    }
}
