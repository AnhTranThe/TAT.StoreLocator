using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IRepositories;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TAT.StoreLocator.Infrastructure.Repositories
{
    //Unit of Work Pattern
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext;
        public IMapper _mapper { get; }
        public IPhotoService _photoService { get; }

        public BaseRepository(AppDbContext dbContext, IMapper mapper, IPhotoService photoService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public IQueryable<T> GetAllQuery()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        public async Task<IEnumerable<T>> GetAllPaging(BasePaginationRequest request)
        {
            IQueryable<T> queryable = _dbContext.Set<T>().AsQueryable();

            // Apply pagination
            queryable = queryable.Skip((request.PageIndex - 1) * request.PageSize)
                                 .Take(request.PageSize);

            return await queryable.ToListAsync();
        }

        public IQueryable<T> GetAllPagingQuery(BasePaginationRequest request)
        {
            IQueryable<T> queryable = _dbContext.Set<T>().AsQueryable();

            // Apply pagination
            queryable = queryable.Skip((request.PageIndex - 1) * request.PageSize)
                                 .Take(request.PageSize);

            return queryable;
        }

        public async Task<T?> GetById(string id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task Add(T entity)
        {
            _ = await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _ = _dbContext.Set<T>().Update(entity);
        }

        public void Remove(T entity)
        {
            _ = _dbContext.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public int Count()
        {
            return _dbContext.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }


    }
}
