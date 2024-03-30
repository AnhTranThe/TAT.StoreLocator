using AutoMapper;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.Core.Mapper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            _ = CreateMap<User, UserResponseModel>().ReverseMap();
            _ = CreateMap<Review, ReviewResponseModel>().ReverseMap();

        }
    }
}

