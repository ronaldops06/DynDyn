namespace Api.Domain.Models
{
    /// <summary>
    /// Objeto de modelo dos valores totais das transações.
    /// </summary>
    public class TransactionTotalModel
    {
        /// <summary>
        /// Total de crédito.
        /// </summary>
        public double Credit { get; set; }

        /// <summary>
        /// Total de débito.
        /// </summary>
        public double Debit { get; set; }

        /// <summary>
        /// Total de transferência.
        /// </summary>
        public double Transfer { get; set; }
    }
}