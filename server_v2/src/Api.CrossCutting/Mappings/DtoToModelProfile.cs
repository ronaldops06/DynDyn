using Api.Domain.Dtos.Balance;
using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Device;
using Api.Domain.Dtos.Operation;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Dtos.Transaction;
using Api.Domain.Enums;
using Api.Domain.Models;
using AutoMapper;
using Domain.Dtos.User;
using Domain.Helpers;
using Domain.Models;

namespace CrossCutting.Mappings
{
    public class DtoToModelProfile : Profile
    {
        private int? _intNullable = null;

        public DtoToModelProfile()
        {
            CreateMap<LoginDto, TransientUserModel>();
            CreateMap<UserRequestDto, TransientUserModel>();
            CreateMap<TransientUserModel, LoginResponseDto>();
            CreateMap<UserDto, UserModel>().ReverseMap();
            CreateMap<UserRequestDto, UserModel>();
            CreateMap<UserModel, UserResponseDto>();
            CreateMap<TransientUserModel, ValidationUserResponseDto>();

            CreateMap<CategoryRequestDto, CategoryModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToEnum<StatusType>()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToEnum<CategoryType>()));
            CreateMap<CategoryModel, CategoryResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type));

            CreateMap<PortfolioRequestDto, PortfolioModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToEnum<StatusType>()))
            .ForMember(dest => dest.ParentPortfolioId, opt => opt.MapFrom(src => src.ParentPortfolio.Id))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id));

            CreateMap<PortfolioModel, PortfolioResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status));
            
            CreateMap<OperationRequestDto, OperationModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToEnum<StatusType>()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToEnum<OperationType>()))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id));
            CreateMap<OperationModel, OperationResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type));

            CreateMap<TransactionRequestDto, TransactionModel>()
            .ForMember(dest => dest.ParentTransactionId, opt => opt.MapFrom(src => (src.ParentTransaction != null) ? src.ParentTransaction.Id : _intNullable))
            .ForMember(dest => dest.PortfolioId, opt => opt.MapFrom(src => src.Portfolio.Id))
            .ForMember(dest => dest.DestinationPortfolioId, opt => opt.MapFrom(src => (src.DestinationPortfolio != null) ? src.DestinationPortfolio.Id : _intNullable))
            .ForMember(dest => dest.OperationId, opt => opt.MapFrom(src => src.Operation.Id))
            .ForMember(dest => dest.Consolidated, opt => opt.MapFrom(src => src.Consolidated ? 1 : 0));
            CreateMap<TransactionModel, TransactionResponseDto>()
            .ForMember(dest => dest.Consolidated, opt => opt.MapFrom(src => (src.Consolidated == SituationType.Sim) ? true : false));
            CreateMap<TransactionTotalModel, TransactionTotalResponseDto>();

            CreateMap<BalanceRequestDto, BalanceModel>()
                .ForMember(dest => dest.PortfolioId, opt => opt.MapFrom(src => src.Portfolio.Id));
            CreateMap<BalanceModel, BalanceResponseDto>();

            CreateMap<DeviceRequestDto, DeviceModel>();
            CreateMap<DeviceModel, DeviceResponseDto>();
        }
    }
}
