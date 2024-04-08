﻿using AutoMapper;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IRepositories;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext dbContext, IMapper mapper, IPhotoService photoService) : base(dbContext, mapper, photoService)
        {

        }


    }
}
