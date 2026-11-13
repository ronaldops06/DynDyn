using Api.Domain.Dtos.Portfolio;

namespace Api.Domain.Dtos.Transaction
{
    /// <summary>
    /// Objeto de transferência de dados para o retorno de impacto da transação nas requisições.
    /// </summary>
    public class TransactionImpactResponseDto : BaseDto
    {
        /// <summary>
        /// Portfólio impactado.
        /// </summary>
        public PortfolioResponseDto Portfolio { get; set; }

        /// <summary>
        /// Atributo do portfólio impactado.
        /// </summary>
        public PortfolioAttributeResponseDto PortfolioAttribute { get; set; }

        /// <summary>
        /// Tipo de impacto.
        /// </summary>
        public int ImpactType { get; set; }

        /// <summary>
        /// Tipo de operação do impacto.
        /// </summary>
        public int OperationType { get; set; }

        /// <summary>
        /// Valor do impacto.
        /// </summary>
        public double Value { get; set; }
    }
}
