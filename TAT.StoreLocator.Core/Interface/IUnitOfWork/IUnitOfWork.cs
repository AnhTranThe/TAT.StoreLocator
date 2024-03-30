using TAT.StoreLocator.Core.Interface.IRepositories;

namespace TAT.StoreLocator.Core.Interface.IUnitOfWork
{
    public interface IUnitOfWork 
    {
        // we have only get because we don't want to set the repository. setting the repository will be done in the UnitOfWork class
        #region IReponstiroy<Entity>
        IProductRepository ProductRepository { get; } 
        ICategoryRepository CategoryRepository { get; }
        IStoreRepository StoreRepository { get; }
        #endregion

        Task CompleteAsync(); // this method will save all the changes made to the database
    }
}
