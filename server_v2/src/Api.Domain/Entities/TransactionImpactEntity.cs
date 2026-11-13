using Api.Domain.Enums;
using Domain.Entities;

namespace Api.Domain.Entities
{
    /// <summary>
    /// Entidade de impactos da transação.
    /// </summary>
    public class TransactionImpactEntity : BaseEntity
    {
        /// <summary>
        /// Identificador da transação associada.
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Transação associada.
        /// </summary>
        public TransactionEntity Transaction { get; set; }

        /// <summary>
        /// Identificador do portfólio impactado.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Portfólio impactado.
        /// </summary>
        public PortfolioEntity Portfolio { get; set; }

        /// <summary>
        /// Identificador do atributo do portfólio impactado.
        /// </summary>
        public int PortfolioAttributeId { get; set; }

        /// <summary>
        /// Atributo do portfólio impactado.
        /// </summary>
        public PortfolioAttributeEntity PortfolioAttribute { get; set; }

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
        
        /// <summary>
        /// Identificador do usuário base.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Usuário base.
        /// </summary>
        public UserEntity User { get; set; }
    }
}
