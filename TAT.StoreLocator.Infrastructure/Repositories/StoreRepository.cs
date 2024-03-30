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
    public class StoreRepository : IStoreRepository

    {

        private readonly AppDbContext _context;
        public StoreRepository(AppDbContext dbContext)
        {
            _context = dbContext;

        }
        public Task<Store> Create(Store model)
        {
            throw new NotImplementedException();
        }

        public Task CreateRange(List<Store> model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Store model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Store>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Store> GetById<Tid>(Tid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponseModel<Store>> GetPaginatedData(PaginationRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExists<Tvalue>(string key, Tvalue value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangeAsync()
        {
            throw new NotImplementedException();
        }

        public Task Update(Store model)
        {
            throw new NotImplementedException();
        }
    }
}
