using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Exceptions;
using TAT.StoreLocator.Core.Interface.IRepositories;
using TAT.StoreLocator.Core.Models.Pagination;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext dbContext)
        {
            _context = dbContext;

        }

        public Task<Product> Create(Product model)
        {
            throw new NotImplementedException();
        }

        public Task CreateRange(List<Product> model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Product model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetById<Tid>(Tid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetByName(string Name)
        {
            if (!string.IsNullOrEmpty(Name) && _context.Products != null)
            {
                List<Product>? data = await _context.Products.Where(t => Name.Contains(t.Name ?? "")).ToListAsync();
                return data;

            }
            throw new NotFoundException($"Entity with Name {Name} not found.");
        }


        public Task<PaginationResponseModel<Product>> GetPaginatedData(PaginationRequestModel request)
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

        public Task Update(Product model)
        {
            throw new NotImplementedException();
        }

    }
}
