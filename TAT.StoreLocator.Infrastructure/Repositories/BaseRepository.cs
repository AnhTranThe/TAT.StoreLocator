using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using TAT.StoreLocator.Core.Exceptions;
using TAT.StoreLocator.Core.Interface.IRepositories;
using TAT.StoreLocator.Core.Models.Pagination;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TAT.StoreLocator.Infrastructure.Repositories
{
    //Unit of Work Pattern
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext;
        protected DbSet<T> DbSet => _dbContext.Set<T>();

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            List<T> data = await _dbContext.Set<T>()
                .AsNoTracking()
                .ToListAsync();

            return data;
        }

        public virtual async Task<PaginationResponseModel<T>> GetPaginatedData(PaginationRequestModel request)
        {
            IQueryable<T> query = _dbContext.Set<T>()
                   .Skip((request.PageIndex - 1) * request.PageSize)
                   .Take(request.PageSize)
                   .AsNoTracking();

            List<T> data = await query.ToListAsync();
            int totalCount = await _dbContext.Set<T>().CountAsync();

            return new PaginationResponseModel<T>(data, totalCount, request.PageSize, request.PageIndex);
        }

        public async Task<T> GetById<Tid>(Tid id)
        {
            T? data = await _dbContext.Set<T>().FindAsync(id);
            return data ?? throw new NotFoundException($"Entity with ID {id} not found.");
        }
       
        public async Task<bool> IsExists<Tvalue>(string key, Tvalue value)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression property = Expression.Property(parameter, key);
            ConstantExpression constant = Expression.Constant(value);
            BinaryExpression equality = Expression.Equal(property, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return await _dbContext.Set<T>().AnyAsync(lambda);
        }

        //Before update existence check
        public async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression property = Expression.Property(parameter, key);
            ConstantExpression constant = Expression.Constant(value);
            BinaryExpression equality = Expression.Equal(property, constant);

            MemberExpression idProperty = Expression.Property(parameter, "Id");
            BinaryExpression idEquality = Expression.NotEqual(idProperty, Expression.Constant(id));

            BinaryExpression combinedExpression = Expression.AndAlso(equality, idEquality);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

            return await _dbContext.Set<T>().AnyAsync(lambda);
        }


        public async Task<T> Create(T model)
        {
            _ = await _dbContext.Set<T>().AddAsync(model);
            _ = await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task CreateRange(List<T> model)
        {
            await _dbContext.Set<T>().AddRangeAsync(model);
            _ = await _dbContext.SaveChangesAsync();
        }

        public async Task Update(T model)
        {
            _ = _dbContext.Set<T>().Update(model);
            _ = await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T model)
        {
            _ = _dbContext.Set<T>().Remove(model);
            _ = await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangeAsync()
        {
            _ = await _dbContext.SaveChangesAsync();
        }



    }
}
