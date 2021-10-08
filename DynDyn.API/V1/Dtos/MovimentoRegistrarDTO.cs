namespace DynDyn.API.V1.Dtos
{
    public class MovimentoRegistrarDTO
    {
        /// <summary>
        /// Identificador e chave do banco de dados
        /// </summary>
        public int ID { get; set; }
        public double Valor { get; set; }
        public string Observacao { get; set; }
        public int ContaID { get; set; }
        public ContaRegistrarDTO Conta { get; set; }
        public OperacaoRegistrarDTO Operacao { get; set; }
    }
}
