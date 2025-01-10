using System;
using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Operation;

namespace Api.Domain.Dtos.Transaction
{
    /// <summary>
    /// Objeto de transferência de dados para o retorno de transação nas requisições.
    /// </summary>
    public class TransactionResponseDto : BaseDto
    {
        /// <summary>
        /// Valor do movimento.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Observação do movimento
        /// </summary>
        public string Observation { get; set; }

        /// <summary>
        /// Indica se o movimento já foi consolidado.
        /// </summary>
        public bool Consolidated { get; set; }

        /// <summary>
        /// Número da parcela referente ao movimento.
        /// </summary>
        public int? Installment { get; set; }

        /// <summary>
        /// Número total de parcelas.
        /// </summary>
        public int? TotalInstallments { get; set; }

        /// <summary>
        /// Data de criação da transação.
        /// </summary>        
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Movimento pai.
        /// </summary>
        public TransactionResponseDto ParentTransaction { get; set; }

        /// <summary>
        /// Conta origem do movimento.
        /// </summary>
        public AccountResponseDto Account { get; set; }

        /// <summary>
        /// Conta destino do movimento (utilizado nas transferências entre contas).
        /// </summary>
        public AccountResponseDto DestinationAccount { get; set; }

        /// <summary>
        /// Operação utilizada no movimento.
        /// </summary>
        public OperationResponseDto Operation { get; set; }
    }
}