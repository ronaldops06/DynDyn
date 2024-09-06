namespace Api.Domain.Dtos.Transaction
{
    /// <summary>
    /// Objeto de transferêncai de dados dos valores totais das transações.
    /// </summary>
    public class TransactionTotalResponseDto
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