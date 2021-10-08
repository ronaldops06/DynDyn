using AutoMapper;
using DynDyn.API.Models;
using DynDyn.API.V1.Dtos;

namespace DynDyn.API.V1.Profiles
{
    public class DyndynProfile : Profile
    {
        public DyndynProfile() {

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
            CreateMap<User, UserAuthDTO>().ReverseMap();
        }
    }
}
