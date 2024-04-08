using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IRepositories;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Interface.IUnitOfWork;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Repositories;

namespace TAT.StoreLocator.Infrastructure.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        private IUserRepository _userRepository;
        private IProductRepository _productRepository;
        private IRoleRepository _roleRepository;


        public UnitOfWork(AppDbContext context, IPhotoService photoService, IMapper mapper)
        {
            _context = context;
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = new UserRepository(context, mapper, photoService);
            _productRepository = new ProductRepository(context, _mapper, _photoService);
            _roleRepository = new RoleRepository(context, _mapper, _photoService);
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context, _mapper, _photoService);
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context, _mapper, _photoService);
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context, _mapper, _photoService);

        public async Task<bool> CommitAsync(string username = "")
        {
            foreach (EntityEntry<BaseEntity> entry in _context.ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                    entry.Entity.CreatedBy = username; // Assuming you want to update CreatedBy for modified entities
                }
                else if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    entry.Entity.CreatedBy = username;
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task RollbackAsync()
        {
            await _context.DisposeAsync();
        }

    }
}
