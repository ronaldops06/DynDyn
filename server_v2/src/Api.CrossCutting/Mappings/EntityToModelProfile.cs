using Api.Domain.Entities;
using Api.Domain.Models;
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
            CreateMap<CategoryEntity, CategoryModel>().ReverseMap();
            CreateMap<PortfolioModel, PortfolioEntity>();
            CreateMap<PortfolioEntity, PortfolioModel>();
            CreateMap<OperationEntity, OperationModel>().ReverseMap();
            CreateMap<TransactionEntity, TransactionModel>().ReverseMap();
            CreateMap<BalanceEntity, BalanceModel>().ReverseMap();
        }
    }
}
