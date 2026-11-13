using Api.Domain.Enums;
using Domain.Models;

namespace Api.Domain.Models
{
    /// <summary>
    /// Objeto de modelo do impacto da transação.
    /// </summary>
    public class TransactionImpactModel : BaseModel
    {
        /// <summary>
        /// Identificador da transação associada.
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Transação associada.
        /// </summary>
        public TransactionModel Transaction { get; set; }

        /// <summary>
        /// Identificador do portfólio impactado.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Portfólio impactado.
        /// </summary>
        public PortfolioModel Portfolio { get; set; }

        /// <summary>
        /// Identificador do atributo do portfólio impactado.
        /// </summary>
        public int PortfolioAttributeId { get; set; }

        /// <summary>
        /// Atributo do portfólio impactado.
        /// </summary>
        public PortfolioAttributeModel PortfolioAttribute { get; set; }

        /// <summary>
        /// Tipo de impacto.
        /// </summary>
        public TransactionImpactType ImpactType { get; set; }

        /// <summary>
        /// Tipo de operação do impacto.
        /// </summary>
        public TransactionImpactOperationType OperationType { get; set; }

        /// <summary>
        /// Valor do impacto.
        /// </summary>
        public double Value { get; set; }
    }
}
