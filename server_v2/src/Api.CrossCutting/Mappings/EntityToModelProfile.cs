using System.Linq;
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
            CreateMap<TransientUserEntity, TransientUserModel>().ReverseMap();
            CreateMap<TransientUserEntity, UserModel>().ReverseMap();
            CreateMap<UserEntity, TransientUserModel>().ReverseMap();
            CreateMap<UserEntity, UserModel>().ReverseMap();
            CreateMap<CategoryEntity, CategoryModel>().ReverseMap();
            CreateMap<PortfolioModel, PortfolioEntity>().ReverseMap();
            CreateMap<OperationEntity, OperationModel>()
                .ForMember(
                    dest => dest.OperationRoles,
                    opt => opt.MapFrom(src => src.OperationRoles.Select(x => x.OperationRole))
                );
            CreateMap<OperationModel, OperationEntity>()
                .ForMember(
                    dest => dest.OperationRoles,
                    opt => opt.MapFrom(src => src.OperationRoles.Select(r => new OperationRoleLinkEntity
                    {
                        OperationId = src.Id,
                        OperationRoleId = r.Id,
                        OperationRole = new OperationRoleEntity
                        {
                            Id = r.Id,
                            Name = r.Name
                        }
                    }))
                );
            CreateMap<TransactionEntity, TransactionModel>().ReverseMap();
            CreateMap<OperationRoleEntity, OperationRoleModel>().ReverseMap();
            CreateMap<BalanceEntity, BalanceModel>().ReverseMap();
            CreateMap<DeviceEntity, DeviceModel>().ReverseMap();
            CreateMap<TrashEntity, TrashModel>().ReverseMap();
            CreateMap<TotalizerRoleEntity, TotalizerRoleModel>()
                .ForMember(
                    dest => dest.OperationRoles,
                    opt => opt.MapFrom(src => src.OperationRoles.Select(x => x.OperationRole))
            );
            CreateMap<TotalizerRoleModel, TotalizerRoleEntity>()
                .ForMember(
                    dest => dest.OperationRoles,
                    opt => opt.MapFrom(src => src.OperationRoles.Select(r => new TotalizerRoleLinkEntity
                    {
                        TotalizerRoleId = src.Id,
                        OperationRoleId = r.Id,
                        OperationRole = new OperationRoleEntity
                        {
                            Id = r.Id,
                            Name = r.Name
                        }
                    }))
                );
        }
    }
}
