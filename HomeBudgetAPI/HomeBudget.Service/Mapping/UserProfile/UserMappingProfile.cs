using AutoMapper;
using HomeBudget.Core.Entities;
using HomeBudget.Service.ModelsDTO.UserModels;

namespace HomeBudget.Service.Mapping.UserProfile
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<NewUserDTO, User>();

            CreateMap<User, UserDTO>();

            CreateMap<User, AppUserDTO>();
        }
    }
}