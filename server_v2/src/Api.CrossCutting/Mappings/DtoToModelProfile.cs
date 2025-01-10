using Api.Domain.Dtos.Category;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using AutoMapper;
using Domain.Dtos.User;
using Domain.Models;
using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Operation;
using Api.Domain.Dtos.Transaction;
using System;
using Api.Domain.Dtos.Balance;

namespace CrossCutting.Mappings
{
    public class DtoToModelProfile : Profile
    {
        private int? _intNullable = null;

        public DtoToModelProfile()
        {
            CreateMap<UserDto, UserModel>().ReverseMap();
            CreateMap<LoginDto, UserModel>().ReverseMap();
            CreateMap<UserResponseDto, UserModel>().ReverseMap();
            CreateMap<UserRequestDto, UserModel>().ReverseMap();

            CreateMap<CategoryRequestDto, CategoryModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToEnum<StatusType>()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToEnum<CategoryType>()));
            CreateMap<CategoryModel, CategoryResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type));

            CreateMap<AccountRequestDto, AccountModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToEnum<StatusType>()))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
            .ForMember(dest => dest.ParentAccountId, opt => opt.MapFrom(src => src.ParentAccount.Id));
            CreateMap<AccountModel, AccountResponseDto>()
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
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Account.Id))
            .ForMember(dest => dest.DestinationAccountId, opt => opt.MapFrom(src => (src.DestinationAccount != null) ? src.DestinationAccount.Id : _intNullable))
            .ForMember(dest => dest.OperationId, opt => opt.MapFrom(src => src.Operation.Id))
            .ForMember(dest => dest.Consolidated, opt => opt.MapFrom(src => src.Consolidated ? 1 : 0));
            CreateMap<TransactionModel, TransactionResponseDto>()
            .ForMember(dest => dest.Consolidated, opt => opt.MapFrom(src => (src.Consolidated == SituationType.Sim) ? true : false));
            CreateMap<TransactionTotalModel, TransactionTotalResponseDto>();

            CreateMap<BalanceRequestDto, BalanceModel>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Account.Id));
            CreateMap<BalanceModel, BalanceResponseDto>();
        }
    }
}
