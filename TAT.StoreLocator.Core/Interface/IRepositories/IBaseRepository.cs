using System.Linq.Expressions;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Interface.IRepositories
{
    //Unit of Work Pattern
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetById(string id);
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> GetAllQuery();
        Task<IEnumerable<T>> GetAllPaging(BasePaginationRequest request);
        IQueryable<T> GetAllPagingQuery(BasePaginationRequest request);
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Update(T entity);
        int Count();
        Task<int> CountAsync();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);




    }
}
