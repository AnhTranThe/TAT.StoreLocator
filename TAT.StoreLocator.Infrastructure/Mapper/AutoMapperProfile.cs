using AutoMapper;
using System.Globalization;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Models.Request.Address;
using TAT.StoreLocator.Core.Models.Request.Review;
using TAT.StoreLocator.Core.Models.Request.Store;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.Authentication;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.Infrastructure.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()

        {
            #region from entities to DTOs

            _ = CreateMap<User, UserResponseModel>()
                 .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.LastName + " " + src.FirstName))
                 .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.ToString("yyyy-MM-ddTHH:mm:sszzz")));

            #endregion from entities to DTOs

            #region from DTOs to entities

            _ = CreateMap<UserResponseModel, User>()
                 .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                 .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => DateTimeOffset.ParseExact(src.Dob, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)));

            _ = CreateMap<LoginResponseModel, User>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserResponseModel.FullName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserResponseModel.UserName))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserResponseModel.Email))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserResponseModel.Id));

            _ = CreateMap<UpdateUserRequestModel, User>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RequestId))
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.LastName + " " + src.FirstName))
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));

            _ = CreateMap<UpdateUserRequestModel, Address>()
              .ForMember(dest => dest.RoadName, opt => opt.MapFrom(src => src.RoadName))
              .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
               .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District));

            _ = CreateMap<UpdateUserRequestModel, Address>()
                  .ForMember(dest => dest.RoadName, opt => opt.MapFrom(src => src.RoadName))
                  .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
                   .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
                  .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                   .ForMember(dest => dest.latitude, opt => opt.MapFrom(src => src.latitude))
                    .ForMember(dest => dest.longitude, opt => opt.MapFrom(src => src.longitude))

                       .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Ward));

            _ = CreateMap<CreateAddressRequestModel, Address>()
                .ForMember(dest => dest.RoadName, opt => opt.MapFrom(src => src.RoadName))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
                 .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                 .ForMember(dest => dest.latitude, opt => opt.MapFrom(src => src.latitude))
                  .ForMember(dest => dest.longitude, opt => opt.MapFrom(src => src.longitude))

                     .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Ward));

            _ = CreateMap<Store, StoreResponseModel>()
                .ForMember(dest => dest.MapGalleryStores, opt => opt.Ignore()) // We handle this manually
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ?
                    new AddressResponseModel
                    {
                        RoadName = src.Address.RoadName,
                        Province = src.Address.Province,
                        District = src.Address.District,
                        Ward = src.Address.Ward,
                        PostalCode = src.Address.PostalCode,
                        Latitude = src.Address.latitude,
                        Longitude = src.Address.longitude
                    } : null));


            _ = CreateMap<StoreRequestModel, Store>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src)) // Map Address property conditionally
                ;
            _ = CreateMap<StoreRequestModel, Address>()
    .ForMember(dest => dest.RoadName, opt => opt.MapFrom(src => src.RoadName))
    .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
    .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
    .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
    .ForMember(dest => dest.latitude, opt => opt.MapFrom(src => src.Latitude))
    .ForMember(dest => dest.longitude, opt => opt.MapFrom(src => src.Longitude))

    ;



            _ = CreateMap<MapGalleryStore, MapGalleryStoreResponseModel>()
              .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.Gallery != null ?
                  src.Gallery.FileName : string.Empty))
              .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Gallery != null ?
                  src.Gallery.Url : string.Empty))
              .ForMember(dest => dest.IsThumbnail, opt => opt.MapFrom(src => src.Gallery != null &&
                  src.Gallery.IsThumbnail));

            _ = CreateMap<ReviewRequestModel, Review>()

                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.RatingValue, opt => opt.MapFrom(src => src.RatingValue))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enums.EReviewStatus.Approved)); // Assuming default status is Pending



            _ = CreateMap<Review, ReviewResponseModel>()
                .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.RatingValue, opt => opt.MapFrom(src => src.RatingValue))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz")))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz")))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product == null ? null : new BaseProductResponseModel
                {
                    Id = src.ProductId ?? "",
                    Name = src.Product.Name,
                    Description = src.Product.Description,
                    Content = src.Product.Content,
                    Price = src.Product.Price,
                    Discount = src.Product.Discount,
                    Quantity = src.Product.Quantity,
                }));



            _ = CreateMap<Review, ReviewResponseModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.RatingValue, opt => opt.MapFrom(src => src.RatingValue))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product == null ? null : new BaseProductResponseModel
                {
                    Id = src.ProductId ?? "",
                    Name = src.Product.Name,
                    Description = src.Product.Description,
                    Content = src.Product.Content,
                    Price = src.Product.Price,
                    Discount = src.Product.Discount,
                    Quantity = src.Product.Quantity,
                }))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz")))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz")))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));

            #endregion from DTOs to entities
        }
    }
}