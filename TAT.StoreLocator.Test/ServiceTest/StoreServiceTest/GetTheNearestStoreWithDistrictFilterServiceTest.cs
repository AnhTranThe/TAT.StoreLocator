
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IPaging;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Store;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Core.Utils;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.StoreServiceTest
{
    public class GetTheNearestStoreWithDistrictFilterServiceTest
    {
        //private static MapperConfiguration ConfigureAutoMapper()
        //{
        //    return new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Store, SimpleStoreResponse>();
        //        cfg.CreateMap<Address, AddressResponseModel>();
        //        cfg.CreateMap<Product, SimpleProductResponse>()
        //           .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.MapGalleryProducts.FirstOrDefault().Gallery.Url));
        //    });
        //}

        //private static List<Store> GetSampleStores()
        //{
        //    return new List<Store>
        //    {
        //        new Store { Id = "store1", Name = "Store 1", AddressId = "address1", Email = "store@example.com", PhoneNumber = "1234567894", IsActive = true, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow },
        //        new Store { Id = "store2", Name = "Store 2", AddressId = "address2", Email = "store2@example.com", PhoneNumber = "1234567892", IsActive = true, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow }
        //    };
        //}

        //private static List<Address> GetSampleAddresses()
        //{
        //    return new List<Address>
        //    {
        //       new Address { Id = "address1", RoadName = "Road 1", Ward = "Ward 1", District = "District 1", Province = "Province 1", latitude = 10762622, longitude = 15600076, PostalCode = "700000" },
        //       new Address { Id = "address2", RoadName = "Road 2", Ward = "Ward 2", District = "District 2", Province = "Province 2", latitude = 10762642, longitude = 15600076, PostalCode = "700001" }
        //    };
        //}

        //private static List<Product> GetSampleProducts()
        //{
        //    return new List<Product>
        //    {
        //        new Product { Id = "product1", Name = "Product 1", StoreId = "store1", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow },
        //        new Product { Id = "product2", Name = "Product 2", StoreId = "store2", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow }
        //    };
        //}

        //private static List<Gallery> GetSampleGalleries()
        //{
        //    return new List<Gallery>
        //    {
        //        new Gallery { Id = "gallery1", Url = "http://example.com/gallery1" },
        //        new Gallery { Id = "gallery2", Url = "http://example.com/gallery2" }
        //    };
        //}

        //private static List<MapGalleryProduct> GetSampleMapGalleryProducts()
        //{
        //    return new List<MapGalleryProduct>
        //    {
        //        new MapGalleryProduct { ProductId = "product1", GalleryId = "gallery1" },
        //        new MapGalleryProduct { ProductId = "product2", GalleryId = "gallery2" }
        //    };
        //}


        //[Fact]
        //public async Task GetTheNearestStore_ReturnsExpectedStores_WithDistrictFilter()
        //{
        //    Arrange
        //   var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        //       .UseInMemoryDatabase(databaseName: $"GetTheNearestStoreAsync_TestDatabase_{Guid.NewGuid()}")
        //       .Options;

        //    using var dbContext = new AppDbContext(dbContextOptions);
        //    dbContext.Addresses.AddRange(GetSampleAddresses());
        //    dbContext.Stores.AddRange(GetSampleStores());
        //    dbContext.Products.AddRange(GetSampleProducts());
        //    dbContext.Galleries.AddRange(GetSampleGalleries());
        //    dbContext.mapGalleryProducts.AddRange(GetSampleMapGalleryProducts());

        //    await dbContext.SaveChangesAsync();

        //    var mapperConfig = ConfigureAutoMapper();
        //    var mapper = mapperConfig.CreateMapper();
        //    var photoServiceMock = new Mock<IPhotoService>();
        //    var loggerMock = new Mock<ILogger<StoreService>>();

        //    var storeService = new StoreService(dbContext, mapper, photoServiceMock.Object, loggerMock.Object);

        //    var getNearestStoreRequest = new GetNearStoreRequestModel
        //    {
        //        Province = "",
        //        District = "District 1",
        //        Ward = "",
        //        keyWord = ""
        //    };

        //    var paginationRequest = new BasePaginationRequest
        //    {
        //        PageIndex = 1,
        //        PageSize = 10
        //    };

        //    Act
        //   var result = await storeService.GetTheNearestStore(getNearestStoreRequest, paginationRequest);

        //    Logging the result for debugging

        //   Console.WriteLine($"Result Success: {result.Success}");
        //   Console.WriteLine($"Result Message: {result.Message}");
        //    if (result.Data != null)
        //    {
        //        foreach (var store in result.Data)
        //        {
        //            Console.WriteLine($"Store: {store.Name}, Province: {store.Address.Province}, District: {store.Address.District}, Ward: {store.Address.Ward}");
        //        }
        //    }

        //    Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.Success, $"Expected success but got {result.Message}");
        //    Assert.NotEmpty(result.Data);
        //    Assert.All(result.Data, store =>
        //    {
        //        string expectedDistrict = "District 1".ToUpper().Trim();
        //        string actualDistrict = CommonUtils.RemoveDiacritics(store.Address.District
        //            .Replace("quan", "", StringComparison.OrdinalIgnoreCase)
        //            .Replace("district", "", StringComparison.OrdinalIgnoreCase))
        //            .Replace("huyen", "", StringComparison.OrdinalIgnoreCase)
        //            .ToUpper()
        //            .Trim();
        //    });
        //}

        private readonly StoreService _storeService;
        private readonly Mock<ILogger<StoreService>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;
        private readonly Mock<IPhotoService> _mockPhotoService;

        public GetTheNearestStoreWithDistrictFilterServiceTest()
        {
            // Mock ILogger
            _mockLogger = new Mock<ILogger<StoreService>>();

            // Setup in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase2")
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
                new Store { Id ="1", Name = "Store 1", Address = new Address { District = "1" }, Products = new List<Product> { new Product { Name = "Product 1" } } },
                new Store { Id = "2", Name = "Store 2", Address = new Address { District = "2" }, Products = new List<Product> { new Product { Name = "Product 2" } } },
                new Store { Id = "3", Name = "Store 3", Address = new Address { District = "3" }, Products = new List<Product> { new Product { Name = "Product 3" } } },
            };

            context.Stores.AddRange(stores);
            context.SaveChanges();
        }

        [Fact]
        public async Task GetTheNearestStoreWithDistrictFilter_ShouldReturnCorrectStores()
        {
            // Arrange
            var getNearStoreRequest = new GetNearStoreRequestModel
            {
                District = "1"
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
            Assert.Contains(response.Data, store => store.Name == "Store 1");
        }
    }
}
