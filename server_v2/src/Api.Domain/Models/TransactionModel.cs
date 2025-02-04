using Api.Domain.Enums;
using Domain.Models;

namespace Api.Domain.Models
{
    /// <summary>
    /// Objeto de modelo da transação.
    /// </summary>
    public class TransactionModel : BaseModel
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
        public SituationType Consolidated { get; set; }

        /// <summary>
        /// Número da parcela referente ao movimento.
        /// </summary>
        public int? Installment { get; set; }

        /// <summary>
        /// Número total de parcelas.
        /// </summary>
        public int? TotalInstallments { get; set; }

        /// <summary>
        /// Identificador do movimento pai.
        /// </summary>
        public int? ParentTransactionId { get; set; }

        /// <summary>
        /// Movimento pai.
        /// </summary>
        public TransactionModel ParentTransaction { get; set; }

        /// <summary>
        /// Identificador da conta origem do movimento.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Conta origem do movimento.
        /// </summary>
        public AccountModel Account { get; set; }

        /// <summary>
        /// Identificador da conta destino do movimento (utilizado nas transferências entre contas).
        /// </summary>
        public int? DestinationAccountId { get; set; }

        /// <summary>
        /// Conta destino do movimento (utilizado nas transferências entre contas).
        /// </summary>
        public AccountModel DestinationAccount { get; set; }

        /// <summary>
        /// Identificador da operação.
        /// </summary>
        public int OperationId { get; set; }

        /// <summary>
        /// Operação utilizada no movimento.
        /// </summary>
        public OperationModel Operation { get; set; }
        
        /// <summary>
        /// Identificador do usuário base.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Usuário base
        /// </summary>
        public UserModel User { get; set; }
    }
}