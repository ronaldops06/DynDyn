namespace Somnia.API.Models
{
    public class Movimento : ModelBase
    {
        public double Valor { get; set; }
        public string Observacao { get; set; }
        public int ContaID { get; set; }
        public Conta Conta { get; set; }
        public int? ContaDestinoID { get; set; }
        public Conta ContaDestino { get; set; }
        public int OperacaoID { get; set; }
        public Operacao Operacao { get; set; }
    }
}
