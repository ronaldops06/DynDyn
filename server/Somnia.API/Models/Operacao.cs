using Somnia.API.Models.Enums;

namespace Somnia.API.Models
{
    public class Operacao : ModelBase
    {
        public string Nome { get; set; }
        public int Recorrente { get; set; }
        public OperacaoTipo Tipo { get; set; }
        public Status Status { get; set; }
        public int CategoriaID { get; set; }
        public Categoria Categoria { get; set; }
    }
}
