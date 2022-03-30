using Somnia.API.Models.Enums;

namespace Somnia.API.Models
{
    public class Movimento : ModelBase
    {
        public double Valor { get; set; }
        public string Observacao { get; set; }
        public Situacao Consolidado { get; set; }
        public int? Parcela { get; set; }
        public int? TotalParcelas { get; set; }
        public int? MovimentoPaiID { get; set; }
        public int ContaID { get; set; }
        public Conta Conta { get; set; }
        public int? ContaDestinoID { get; set; }
        public Conta ContaDestino { get; set; }
        public int OperacaoID { get; set; }
        public Operacao Operacao { get; set; }
    }
}
