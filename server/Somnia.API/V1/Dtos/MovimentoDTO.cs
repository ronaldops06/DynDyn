using Somnia.API.Models.Enums;
using System;

namespace Somnia.API.V1.Dtos
{
    public class MovimentoDTO
    {
        /// <summary>
        /// Identificador e chave do banco de dados
        /// </summary>
        public int ID { get; set; }
        public double Valor { get; set; }
        public string Observacao { get; set; }
        public Situacao Consolidado { get; set; }
        public int? Parcela { get; set; }
        public int? TotalParcelas { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public int MovimentoPaiID { get; set; }
        public int ContaID { get; set; }
        public ContaDTO Conta { get; set; }
        public int ContaDestinoID { get; set; }
        public ContaRegistrarDTO ContaDestino { get; set; }
        public int OperacaoID { get; set; }
        public OperacaoDTO Operacao { get; set; }
    }
}
