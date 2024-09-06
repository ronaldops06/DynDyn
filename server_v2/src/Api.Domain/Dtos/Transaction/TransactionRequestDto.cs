using System;
using System.ComponentModel.DataAnnotations;
using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Operation;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.Transaction
{
    /// <summary>
    /// Objeto de transferência de dados para o recebimento de transação nas requisições.
    /// </summary>
    public class TransactionRequestDto : BaseDto
    {
        /// <summary>
        /// Valor do movimento.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(0.000001, double.MaxValue)]
        public double Value { get; set; }

        /// <summary>
        /// Observação do movimento
        /// </summary>
        [StringLength(200, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Observation { get; set; }

        /// <summary>
        /// Indica se o movimento já foi consolidado.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
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
        /// Movimento pai.
        /// </summary>
        public TransactionRequestDto ParentTransaction { get; set; }

        /// <summary>
        /// Conta origem do movimento.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public AccountRequestDto Account { get; set; }

        /// <summary>
        /// Conta destino do movimento (utilizado nas transferências entre contas).
        /// </summary>
        public AccountRequestDto DestinationAccount { get; set; }

        /// <summary>
        /// Operação utilizada no movimento.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public OperationRequestDto Operation { get; set; }

        /// <summary>
        /// Data de criação da transação.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public DateTime DataCriacao { get; set; }
    }
}