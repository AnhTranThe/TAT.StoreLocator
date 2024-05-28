using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Store;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.StoreServiceTest
{
    public class GetTheNearestStoreWithWardFST
    {
        private readonly StoreService _storeService;
        private readonly Mock<ILogger<StoreService>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;
        private readonly Mock<IPhotoService> _mockPhotoService;

        public GetTheNearestStoreWithWardFST()
        {
            // Mock ILogger
            _mockLogger = new Mock<ILogger<StoreService>>();

            // Setup in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new AppDbContext(options);

            // Configure AutoMapper
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Store, SimpleStoreResponse>();
            });
            _mapper = config.CreateMapper();

            // Mock IPhotoService
            _mockPhotoService = new Mock<IPhotoService>();

            // Initialize StoreService
            _storeService = new StoreService(_dbContext, _mapper, _mockPhotoService.Object, _mockLogger.Object);

            // Seed data
            SeedData(_dbContext);
        }

        private void SeedData(AppDbContext context)
        {
            var stores = new List<Store>
            {
                new Store { Id ="1", Name = "Store 1", Address = new Address { Ward = "Ward 1", District = "District 1" }, Products = new List<Product> { new Product { Name = "Product 1" } } },
                new Store { Id = "2", Name = "Store 2", Address = new Address { Ward = "Ward 2", District = "District 2" }, Products = new List<Product> { new Product { Name = "Product 2" } } },
                new Store { Id = "3", Name = "Store 3", Address = new Address { Ward = "Ward 3", District = "District 3" }, Products = new List<Product> { new Product { Name = "Product 3" } } },
            };

            context.Stores.AddRange(stores);
            context.SaveChanges();
        }

        [Fact]
        public async Task GetTheNearestStoreWithWardFilter_ShouldReturnCorrectStores()
        {
            // Arrange
            var getNearStoreRequest = new GetNearStoreRequestModel
            {
                Ward = "Ward 1"
            };
            var paginationRequest = new BasePaginationRequest
            {
                PageIndex = 1,
                PageSize = 10
            };

            // Act
            var response = await _storeService.GetTheNearestStore(getNearStoreRequest, paginationRequest);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(System.Net.HttpStatusCode.OK.ToString(), response.Code);
            Assert.NotEmpty(response.Data);
            Assert.Contains(response.Data, store => store.Address.Ward == "Ward 1");
        }
    }
}
