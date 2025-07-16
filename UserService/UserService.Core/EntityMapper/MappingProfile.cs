using AutoMapper;
using UserService.Core.Dto;
using UserService.Database.BusinessEntity;

namespace UserService.Core.EntityMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<CreateUserRequestDto, UserData>().ReverseMap();
            CreateMap<UpdateUserRequestDto, UserData>().ReverseMap();
            CreateMap<UserDetailsResponseDto, UserData>().ReverseMap();
        }
    }
}
