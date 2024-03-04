using AutoMapper;
using Domain.Dtos.User;
using Domain.Models;

namespace CrossCutting.Mappings
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<UserDto, UserModel>().ReverseMap();
            CreateMap<LoginDto, UserModel>().ReverseMap();
            CreateMap<UserResponseDto, UserModel>().ReverseMap();
            CreateMap<UserRequestDto, UserModel>().ReverseMap();
        }
    }
}
