using AutoMapper;
using Domain.Models;

namespace CrossCutting.Mappings
{
    public class ModelToModelProfile : Profile
    {
        public ModelToModelProfile()
        {
            CreateMap<TransientUserModel, UserModel>().ReverseMap();
        }
    }
}