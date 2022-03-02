using Somnia.API.Models.Enums;

namespace Somnia.API.Models
{
    public class Conta : ModelBase
    {
        public string Nome { get; set; }
        public Status Status { get; set; } = Status.Ativo;
        public int CategoriaID { get; set; }
        public Categoria Categoria { get; set; }
        public int? ContaPaiID { get; set; }
        public Conta ContaPai { get; set; }
    }
}
