using System.Collections.Generic;
using System.Linq;
using Api.Domain.Enums;
using Api.Domain.Models;
using AutoMapper;

namespace Api.CrossCutting.Mappings
{
    public class DictionaryToModelProfile : Profile
    {
        public DictionaryToModelProfile()
        {
            CreateMap<Dictionary<OperationType, double>, TransactionTotalModel>()
            .ForMember(dest => dest.Credit, opt => opt.MapFrom(src => src.FirstOrDefault(x => x.Key.Equals(OperationType.Credito)).Value))
            .ForMember(dest => dest.Debit, opt => opt.MapFrom(src => src.FirstOrDefault(x => x.Key.Equals(OperationType.Debito)).Value))
            .ForMember(dest => dest.Transfer, opt => opt.MapFrom(src => src.FirstOrDefault(x => x.Key.Equals(OperationType.Transferencia)).Value));
        }
    }
}