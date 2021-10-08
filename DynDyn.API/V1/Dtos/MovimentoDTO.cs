namespace DynDyn.API.V1.Dtos
{
    public class MovimentoDTO
    {
        /// <summary>
        /// Identificador e chave do banco de dados
        /// </summary>
        public int ID { get; set; }
        public double Valor { get; set; }
        public string Observacao { get; set; }
        public int ContaID { get; set; }
        public ContaDTO Conta { get; set; }
        public OperacaoDTO Operacao { get; set; }
    }
}
