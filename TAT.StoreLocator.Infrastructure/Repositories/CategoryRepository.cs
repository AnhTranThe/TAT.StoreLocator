using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IRepositories;
using TAT.StoreLocator.Core.Models.Pagination;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext dbContext)
        {
            _context = dbContext;

        }
        Task<Category> IBaseRepository<Category>.Create(Category model)
        {
            throw new NotImplementedException();
        }

        Task IBaseRepository<Category>.CreateRange(List<Category> model)
        {
            throw new NotImplementedException();
        }

        Task IBaseRepository<Category>.Delete(Category model)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Category>> IBaseRepository<Category>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<Category> IBaseRepository<Category>.GetById<Tid>(Tid id)
        {
            throw new NotImplementedException();
        }

        Task<PaginationResponseModel<Category>> IBaseRepository<Category>.GetPaginatedData(PaginationRequestModel request)
        {
            throw new NotImplementedException();
        }

        Task<bool> IBaseRepository<Category>.IsExists<Tvalue>(string key, Tvalue value)
        {
            throw new NotImplementedException();
        }

        Task<bool> IBaseRepository<Category>.IsExistsForUpdate<Tid>(Tid id, string key, string value)
        {
            throw new NotImplementedException();
        }

        Task IBaseRepository<Category>.SaveChangeAsync()
        {
            throw new NotImplementedException();
        }

        Task IBaseRepository<Category>.Update(Category model)
        {
            throw new NotImplementedException();
        }
    }
}
