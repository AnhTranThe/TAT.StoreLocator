﻿using AutoMapper;
using System.Globalization;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Models.Request.Address;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.Authentication;
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Core.Models.Response.User;
using TAT.StoreLocator.Core.Models.Request.Review;
using static TAT.StoreLocator.Core.Helpers.Enums;
using TAT.StoreLocator.Core.Models.Response.Address; // test
using TAT.StoreLocator.Core.Helpers;

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


            #endregion


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


            CreateMap<Store, StoreResponseModel>()
            .ForMember(dest => dest.MapGalleryStores, opt => opt.Ignore()); // We handle this manually
            CreateMap<MapGalleryStore, MapGalleryStoreResponse>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.Gallery.FileName))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Gallery.Url))
                .ForMember(dest => dest.IsThumbnail, opt => opt.MapFrom(src => src.Gallery.IsThumbnail));


  
            CreateMap<Review, ReviewResponseModel>()
            .ForMember(dest => dest.Store, opt => opt.MapFrom(src => src.Product.Store));
            CreateMap<Store, StoreResponse>();


            CreateMap<CreateReviewRequestModel, Review>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.RatingValue, opt => opt.MapFrom(src => src.RatingValue))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enums.EReviewStatus.Pending)) // Assuming default status is Pending
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());


            CreateMap<UpdateReviewRequestModel, Review>()
                  .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.RatingValue, opt => opt.MapFrom(src => src.RatingValue))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ?? Enums.EReviewStatus.Pending)) // Assuming default status is Pending
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            //.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
            #endregion



        }
    }
}


