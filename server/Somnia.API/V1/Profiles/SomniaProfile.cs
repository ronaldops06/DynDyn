using AutoMapper;
using Somnia.API.Models;
using Somnia.API.V1.Dtos;

namespace Somnia.API.V1.Profiles
{
    public class SomniaProfile : Profile
    {
        public SomniaProfile() {

            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaRegistrarDTO>().ReverseMap();

            CreateMap<Conta, ContaDTO>().ReverseMap();
            CreateMap<Conta, ContaRegistrarDTO>().ReverseMap();

            CreateMap<Operacao, OperacaoDTO>().ReverseMap();
            CreateMap<Operacao, OperacaoRegistrarDTO>().ReverseMap();

            CreateMap<Movimento, MovimentoDTO>().ReverseMap();
            CreateMap<Movimento, MovimentoRegistrarDTO>().ReverseMap();

            CreateMap<Saldo, SaldoDTO>().ReverseMap();
            CreateMap<Saldo, SaldoRegistrarDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserRegistrarDTO>().ReverseMap();
            CreateMap<User, UserAuthDTO>().ReverseMap();
        }
    }
}
