using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace CrossCutting.Mappings
{
    public class EntityToModelProfile : Profile
    {
        public EntityToModelProfile()
        {
            CreateMap<UserEntity, LoginModel>().ReverseMap();
            CreateMap<UserEntity, UserModel>().ReverseMap();
        }
    }
}
