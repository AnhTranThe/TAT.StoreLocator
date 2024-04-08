using TAT.StoreLocator.Core.Interface.IRepositories;

namespace TAT.StoreLocator.Core.Interface.IUnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IProductRepository ProductRepository { get; }
        IRoleRepository RoleRepository { get; }
        Task<bool> CommitAsync(string username = "");
        Task RollbackAsync();

    }
}
