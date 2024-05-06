using AutoMapper;
using System.Globalization;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Models.Request.Address;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.Authentication;
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

            #endregion

        }
    }
}
