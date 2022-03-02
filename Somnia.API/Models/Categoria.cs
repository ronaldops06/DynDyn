using Somnia.API.Models.Enums;

namespace Somnia.API.Models
{
    public class Categoria : ModelBase
    {
        public string Nome { get; set; }
        public CategoriaTipo Tipo { get; set; }
        public Status Status { get; set; } = Status.Ativo;
    }
}
