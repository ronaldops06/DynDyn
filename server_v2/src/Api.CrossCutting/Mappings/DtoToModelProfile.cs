using Api.Domain.Dtos.Category;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
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
            CreateMap<CategoryRequestDto, CategoryModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToEnum<StatusType>()))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToEnum<CategoryType>()));
            CreateMap<CategoryModel, CategoryResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (int)src.Tipo));
        }
    }
}
