using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Core.Interface.IRepositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<Product>> GetByName(string Name);

    }
}
